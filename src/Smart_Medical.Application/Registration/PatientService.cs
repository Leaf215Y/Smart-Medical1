using Smart_Medical.DoctorvVsit;
using Smart_Medical.Medical;
using Smart_Medical.OutpatientClinic.Dtos;
using Smart_Medical.OutpatientClinic.Dtos.Parameter;
using Smart_Medical.OutpatientClinic.IServices;
using Smart_Medical.Patient;
using Smart_Medical.Until;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

public class PatientService : ApplicationService, IPatientService
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    /// <summary>
    /// 就诊流程
    /// </summary>
    private readonly IRepository<DoctorClinic, Guid> _doctorclinRepo;
    /// <summary>
    /// 患者基本信息
    /// </summary>
    private readonly IRepository<BasicPatientInfo, Guid> _patientRepo;
    /// <summary>
    /// 患者病历信息
    /// </summary>
    private readonly IRepository<Sick, Guid> _sickRepo;
    /// <summary>
    /// 患者开具处方
    /// </summary>
    private readonly IRepository<PatientPrescription, Guid> _prescriptionRepo;

    public PatientService(
        IUnitOfWorkManager unitOfWorkManager, IRepository<DoctorClinic, Guid> doctorclinRepo, IRepository<BasicPatientInfo, Guid> basicpatientRepo, IRepository<Sick, Guid> sickRepo, IRepository<PatientPrescription, Guid> prescriptionRepo)
    {
        _unitOfWorkManager = unitOfWorkManager;
        _doctorclinRepo = doctorclinRepo;
        _patientRepo = basicpatientRepo;
        _sickRepo = sickRepo;
        _prescriptionRepo = prescriptionRepo;
    }

    /// <summary>
    /// 快速就诊、登记患者信息
    /// Quick registration and patient information entry
    /// </summary>
    /// <remarks>
    /// POST /api/app/patient/registration-patient
    /// 用于快速登记患者基本信息并创建就诊流程、病历信息。
    /// </remarks>
    public async Task<ApiResult> RegistrationPatientAsync(InsertPatientDto input)
    {
        using (var uow = _unitOfWorkManager.Begin(requiresNew: true))
        {
            try
            {
                // ================================
                //  1. 创建患者基本信息
                // ================================

                BasicPatientInfo patientInfo = null;

                // 查询患者是否已存在（根据身份证号）
                bool exists = await _patientRepo.AnyAsync(x => x.IdNumber == input.IdNumber);

                // 映射 DTO 到实体
                BasicPatientInfo patient = ObjectMapper.Map<InsertPatientDto, BasicPatientInfo>(input);
                patient.VisitStatus = "待就诊"; // 初始状态设为"待就诊"

                // 患者信息不存在则插入，存在则更新
                if (!exists)
                    patientInfo = await _patientRepo.InsertAsync(patient);
                else
                    patientInfo = await _patientRepo.UpdateAsync(patient);

                // 判断是否成功
                if (patientInfo == null)
                {
                    return ApiResult.Fail("患者信息登记失败，请稍后重试！", ResultCode.Error);
                }

                // ================================
                //  2. 创建就诊流程记录
                // ================================
                DoctorClinic doctorClinic = ObjectMapper.Map<InsertPatientDto, DoctorClinic>(input);
                doctorClinic.PatientId = patient.Id;         // 关联患者 ID
                doctorClinic.DispensingStatus = 0;           // 默认"未发药"

                var isClinicInserted = await _doctorclinRepo.InsertAsync(doctorClinic) != null;
                if (!isClinicInserted)
                {
                    return ApiResult.Fail("就诊流程创建失败，请稍后重试！", ResultCode.Error);
                }
/*
                // ================================
                // 3. 创建患者病历信息（暂为空）
                // ================================
                Sick sick = ObjectMapper.Map<InsertPatientDto, Sick>(input);
                sick.AdmissionDiagnosis = "";                      // 初始为空
                sick.DischargeDepartment = "";                     // 初始为空
                sick.DischargeDiagnosis = patient.Id.ToString();   // 绑定患者 ID
                sick.InpatientNumber = "";                         // 暂无住院号
                sick.Status = "";                                  // 暂无状态
                sick.Name = patient.PatientName;                   // 冗余存下姓名方便查询*/
/*
                var isSickInserted = await _sickRepo.InsertAsync(sick) != null;
                if (!isSickInserted)
                {
                    return ApiResult.Fail("患者病历信息创建失败，请稍后重试！", ResultCode.Error);
                }
*/
                // ================================
                //  提交整个事务
                // ================================
                await uow.CompleteAsync(); // 所有插入成功才会走到这里，提交事务

                return ApiResult.Success(ResultCode.Success); // 返回成功结果

            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    /// <summary>
    /// 就诊患者
    /// Visiting patients (list)
    /// </summary>
    /// <remarks>
    /// POST /api/app/patient/visiting-patients
    /// 分页获取待就诊或已就诊的患者列表，可按关键词模糊查询。
    /// </remarks>
    /// <param name="input">参数列表，包含分页和关键词</param>
    /// <returns></returns>
    public async Task<ApiResult<PagedResultDto<GetVisitingDto>>> VisitingPatientsAsync(GetVistingParameterDtos input)
    {
        try
        {
            // 1. 转换 VisitStatus 参数为字符串
            string status = input.VisitStatus switch
            {
                1 => "待就诊",
                2 => "已就诊",
                _ => throw new Exception("就诊信息异常")
            };

            // 2. 获取 IQueryable 查询对象（还没执行数据库）
            var query = await _patientRepo.GetQueryableAsync();

            // 3. 根据 VisitStatus 筛选患者
            var filteredQuery = query.Where(x => x.VisitStatus == status);

            // 4. 关键词模糊匹配身份证号、姓名、电话
            if (!string.IsNullOrWhiteSpace(input.Keyword))
            {
                string keyword = input.Keyword.Trim();
                filteredQuery = filteredQuery.Where(x =>
                    x.IdNumber.Contains(keyword) ||
                    x.PatientName.Contains(keyword) ||
                    x.ContactPhone.Contains(keyword)
                );
            }

            // 5. 计算总条数（分页用）
            var totalCount = await AsyncExecuter.CountAsync(filteredQuery);

            // 6. 分页查询，跳过前面页数的数据，取当前页数据
            var pagedPatients = await AsyncExecuter.ToListAsync(
                filteredQuery
                    .Skip((input.PageIndex - 1) * input.PageSize)
                    .Take(input.PageSize)
            );

            // 7. 实体转 DTO
            var result = ObjectMapper.Map<List<BasicPatientInfo>, List<GetVisitingDto>>(pagedPatients);

            // 8. 返回分页结果
            return ApiResult<PagedResultDto<GetVisitingDto>>.Success(
                new PagedResultDto<GetVisitingDto>(totalCount, result),
                ResultCode.Success
            );
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// 就诊患者详细信息
    /// Get patient detail info
    /// </summary>
    /// <remarks>
    /// GET /api/app/patient/patient-info/{patientId}
    /// 获取指定患者的详细基本信息。
    /// </remarks>
    /// <param name="patientId">患者id</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<ApiResult<BasicPatientInfoDto>> GetPatientInfoAsync(Guid patientId)
    {
        try
        {
            var patientInfo = await _patientRepo.FindAsync(patientId);
            if (patientInfo == null)
            {
                return ApiResult<BasicPatientInfoDto>.Fail("未找到该患者信息", ResultCode.NotFound);
            }
            var result = ObjectMapper.Map<BasicPatientInfo, BasicPatientInfoDto>(patientInfo);
            return ApiResult<BasicPatientInfoDto>.Success(result, ResultCode.Success);
        }
        catch (Exception)
        {

            throw;
        }
    }


    /// <summary>
    /// 患者所有病历信息
    /// Get all medical records for a patient
    /// </summary>
    /// <remarks>
    /// GET /api/app/patient/patient-sick-info/{patientId}
    /// 获取指定患者的所有病历信息。
    /// </remarks>
    /// <param name="patientId">病历外键</param>
    /// <returns></returns>
    public async Task<ApiResult<List<GetSickInfoDto>>> GetPatientSickInfoAsync(Guid patientId)
    {
        /* try
         {
             var sickList = await _sickRepo.GetListAsync(x => x.DischargeDiagnosis == patientId.ToString());
             if (sickList == null || !sickList.Any())
             {
                 return ApiResult<List<GetSickInfoDto>>.Fail("未找到该患者病历信息", ResultCode.NotFound);
             }
             var result = ObjectMapper.Map<List<Sick>, List<GetSickInfoDto>>(sickList);
             return ApiResult<List<GetSickInfoDto>>.Success(result, ResultCode.Success);
         }
         catch (Exception)
         {
             throw;
         }*/
        throw new Exception();
    }

    /// <summary>
    /// 开具处方
    /// Doctor's prescription
    /// </summary>
    /// <remarks>
    /// POST /api/app/patient/doctors-prescription
    /// 医生为患者开具处方，支持多药品批量录入。
    /// </remarks>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<ApiResult> DoctorsPrescription(DoctorPrescriptionDto input)
    {
        try
        {
            if (input == null || input.PrescriptionItems == null || !input.PrescriptionItems.Any())
                return ApiResult.Fail("处方信息不能为空！", ResultCode.Error);

            // 统一的 PrescriptionId，代表一次处方
            var prescriptionId = new Random().Next(100000, 999999);

            using (var uow = _unitOfWorkManager.Begin())
            {
                foreach (var item in input.PrescriptionItems)
                {
                    var prescription = new PatientPrescription
                    {
                        PatientNumber = input.PatientNumber,
                        PrescriptionTemplateNumber = input.PrescriptionTemplateNumber,
                    /*    MedicationName = item.MedicationName,
                        Specification = item.Specification,
                        UnitPrice = item.UnitPrice,
                        Dosage = item.Dosage,
                        DosageUnit = item.DosageUnit,
                        Usage = item.Usage,
                        Frequency = item.Frequency,
                        Number = item.Number,
                        NumberUnit = item.NumberUnit,
                        MedicalAdvice = item.MedicalAdvice,
                        TotalPrice = item.UnitPrice * item.Number,
                        PrescriptionId = prescriptionId*/
                    };

                    var exists = await _prescriptionRepo.InsertAsync(prescription);
                }

                await uow.CompleteAsync();
            }
            //如果没有抛出异常,说明事务提交成功,如果抛出异常,说明事务会被自动回滚。
            //开具处方后，将患者的就诊状态更新为“已就诊”，流程信息也会被更新
            var patient = await _patientRepo.GetAsync(input.PatientNumber);
            patient.VisitStatus = "已就诊"; // 更新患者状态为“已就诊”
            await _patientRepo.UpdateAsync(patient);

            var doctorClinic = await _doctorclinRepo.FirstOrDefaultAsync(x => x.PatientId == input.PatientNumber);


            return ApiResult.Success(ResultCode.Success);
        }
        catch (Exception)
        {

            throw;
        }
    }
}




