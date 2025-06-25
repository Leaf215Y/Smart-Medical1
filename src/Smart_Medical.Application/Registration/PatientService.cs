using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Smart_Medical.DoctorvVsit;
using Smart_Medical.Medical;
using Smart_Medical.OutpatientClinic.Dtos;
using Smart_Medical.OutpatientClinic.Dtos.Parameter;
using Smart_Medical.OutpatientClinic.IServices;
using Smart_Medical.Patient;
using Smart_Medical.Pharmacy;
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

namespace Smart_Medical.Registration
{
    /// <summary>
    /// 医疗管理
    /// </summary>
    [ApiExplorerSettings(GroupName = "医疗管理")]
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
        /// <summary>
        /// 药品
        /// </summary>
        private readonly IRepository<Drug, int> _drugRepo;

        public PatientService(
            IUnitOfWorkManager unitOfWorkManager, IRepository<DoctorClinic, Guid> doctorclinRepo, IRepository<BasicPatientInfo, Guid> basicpatientRepo, IRepository<Sick, Guid> sickRepo, IRepository<PatientPrescription, Guid> prescriptionRepo, IRepository<Drug, int> drugRepo)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _doctorclinRepo = doctorclinRepo;
            _patientRepo = basicpatientRepo;
            _sickRepo = sickRepo;
            _prescriptionRepo = prescriptionRepo;
            _drugRepo = drugRepo;
        }

        /// <summary>
        /// 快速就诊、登记患者信息
        /// </summary>
        /// <param name="input">患者就诊登记信息</param>
        /// <returns>接口返回结果</returns>
        public async Task<ApiResult> RegistrationPatientAsync(InsertPatientDto input)
        {
            try
            {
                using (var uow = _unitOfWorkManager.Begin(requiresNew: true))
                {
                    try
                    {
                        // ================================
                        // 1. 创建或更新患者基本信息
                        // ================================

                        var existingPatient = await _patientRepo.FirstOrDefaultAsync(x => x.IdNumber == input.IdNumber);

                        BasicPatientInfo patient;

                // 映射 DTO 到实体
                BasicPatientInfo patient = ObjectMapper.Map<InsertPatientDto, BasicPatientInfo>(input);
                patient.VisitStatus = "待就诊"; // 初始状态设为"待就诊"
                        if (existingPatient == null)
                        {
                            // ---- 新建患者信息 ----
                            patient = ObjectMapper.Map<InsertPatientDto, BasicPatientInfo>(input);
                            patient.VisitStatus = "待就诊";
                            patient = await _patientRepo.InsertAsync(patient);
                        }
                        else
                        {
                            // ---- 更新已有患者信息 ----
                            ObjectMapper.Map(input, existingPatient);
                            patient = await _patientRepo.UpdateAsync(existingPatient);
                        }

                        if (patient == null)
                            return ApiResult.Fail("患者信息登记失败，请稍后重试！", ResultCode.Error);

                        // ================================
                        // 2. 创建就诊流程记录
                        // ================================

                        var doctorClinic = ObjectMapper.Map<InsertPatientDto, DoctorClinic>(input);

                        doctorClinic.PatientId = patient.Id;
                        doctorClinic.VisitDateTime = input.VisitDate ?? DateTime.Now;
                        doctorClinic.ExecutionStatus = ExecutionStatus.PendingConsultation;
                        doctorClinic.DispensingStatus = 0;
                        doctorClinic.VisitType = existingPatient == null ? "初诊" : "复诊";

                        if (await _doctorclinRepo.InsertAsync(doctorClinic) == null)
                            return ApiResult.Fail("就诊流程创建失败，请稍后重试！", ResultCode.Error);

                        // ================================
                        // 3. 创建患者病历信息（初始化）
                        // ================================

                        var sick = new Sick
                        {
                            BasicPatientId = patient.Id,
                            Status = "新建",              // 病历状态必须有值，防止 Required 报错
                            InpatientNumber = "",

                            AdmissionDiagnosis = "",
                            DischargeTime = DateTime.Now,

                            Temperature = 36.5M,
                            Pulse = 75,
                            Breath = 18,
                            BloodPressure = "120/80"
                        };

                        if (await _sickRepo.InsertAsync(sick) == null)
                            return ApiResult.Fail("病历信息创建失败，请稍后重试！", ResultCode.Error);

                        // ================================
                        // 4. 提交事务，结束登记流程
                        // ================================

                        await uow.CompleteAsync();

                        return ApiResult.Success(ResultCode.Success);
                    }
                    catch (Exception ex)
                    {
                        // ================================
                        // 异常处理（输出异常信息）
                        // ================================
                        return ApiResult.Fail("系统异常：" + ex.Message, ResultCode.Error);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

        }


        /// <summary>
        /// 就诊患者
        /// </summary>
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
                    //  _   "弃元"  :  "匹配所有不符合前面条件的值"
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
        /// </summary>
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

                var result = ObjectMapper.Map<BasicPatientInfo, BasicPatientInfoDto>(patientInfo);
                return ApiResult<BasicPatientInfoDto>.Success(result, ResultCode.Success);
            }
            catch (Exception)
            {

                throw;
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

        /// <summary>
        /// 患者所有病历信息
        /// </summary>
        /// <param name="patientId">患者id</param>
        /// <returns></returns>
        public async Task<ApiResult<List<GetSickInfoDto>>> GetPatientSickInfoAsync(Guid patientId)
        {
            try
            {
                //查询流程表是否为初诊，初诊没有病历信息


                // 获取患者基本信息数据
                var patients = await _patientRepo.GetQueryableAsync();

                // 获取就诊记录数据
                var clinics = await _doctorclinRepo.GetQueryableAsync();

                // 获取病历数据
                var sicks = await _sickRepo.GetQueryableAsync();

                // 获取处方数据
                var prescriptions = await _prescriptionRepo.GetQueryableAsync();

                //详细的处方信息需要读取存储的json数据                

                //linq联查本身没有问题，在病历表中如果有数据才能执行
                var query = from patient in patients
                            where patient.Id == patientId
                            join sicks1 in sicks
                            on patient.Id equals sicks1.BasicPatientId
                            join clinic in clinics
                            on patient.Id equals clinic.PatientId
                            join prescription in prescriptions
                            on patientId equals prescription.PatientNumber
                            select new GetSickInfoDto
                            {

                            };


                //查询
                var result = await AsyncExecuter.ToListAsync(query);
                if (result == null)
                    ApiResult.Fail("患者病历不存在", ResultCode.NotFound);
                return ApiResult<List<GetSickInfoDto>>.Success(result, ResultCode.Success);
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
            catch (Exception ex)
            {
                return ApiResult<List<GetSickInfoDto>>.Fail("系统异常：" + ex.Message, ResultCode.Error);
            }
        }

        /// <summary>
        /// 开具处方
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ApiResult> DoctorsPrescription(DoctorPrescriptionDto input)
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
            try
            {
                //判断输入参数是否完整
                if (input == null || input.PatientNumber == Guid.Empty)
                    return ApiResult.Fail("患者信息不完整！", ResultCode.Error);

                //如果使用处方模板，则必须提供模板编号
                if (input.IsActive)
                {
                    if (input.PrescriptionItems == null)
                    {
                        return ApiResult.Fail("处方项不能为空！", ResultCode.Error);
                    }
                }

                // 获取患者基本信息
                var patient = await _patientRepo.FindAsync(input.PatientNumber);
                if (patient == null)
                    return ApiResult.Fail("未找到该患者信息！", ResultCode.NotFound);

                //生成统一的处方 ID
                var prescriptionId = Guid.NewGuid();

                // =============================================
                // 根据 PrescriptionItems 插入处方明细表
                // =============================================

                //判断每种药品是否存在、库存是否充足
                foreach (var item in input.PrescriptionItems)
                {
                    // 获取药品信息
                    var drug = await _drugRepo.FirstOrDefaultAsync(d => d.Id == item.DrugId);
                    //药品不存在返回错误信息
                    if (drug == null)
                        return ApiResult.Fail($"未找到药品ID为 {item.DrugId} 的药品信息", ResultCode.Error);

                    //查找的药品库存是否充足
                    int remainingStock = drug.Stock - item.Number;
                    if (remainingStock < 0)
                        return ApiResult.Fail($"药品 {drug.DrugName} 库存不足，无法开具处方", ResultCode.Error);

                    //提前 return，但没有调用 uow.CompleteAsync()，那事务是不会提交的，会自动回滚

                    // 更新药品库存
                    drug.Stock = remainingStock;
                    await _drugRepo.UpdateAsync(drug);
                }

                //创建处方记录
                var prescription = new PatientPrescription
                {
                    PrescriptionTemplateNumber = input.PrescriptionTemplateNumber,
                    PatientNumber = input.PatientNumber,
                    IsActive = input.IsActive,
                    // 序列化处方明细
                    DrugIds = JsonConvert.SerializeObject(input.PrescriptionItems),
                    MedicalAdvice = input.MedicalAdvice
                };
                //保存处方记录
                await _prescriptionRepo.InsertAsync(prescription);

                //更新患者状态为“已就诊”
                patient.VisitStatus = "已就诊";
                //更新 DoctorClinic 表的状态字段 
                //判断状态为“待就诊”
                var doctorClinic = await _doctorclinRepo.FirstOrDefaultAsync(
                    x => x.PatientId == input.PatientNumber &&
                    x.ExecutionStatus == ExecutionStatus.PendingConsultation
                    );

                if (doctorClinic == null)
                    return ApiResult.Fail("未找到就诊记录！", ResultCode.NotFound);

                // 更新就诊记录状态为“已就诊”
                doctorClinic.ExecutionStatus = ExecutionStatus.Completed;
                await _doctorclinRepo.UpdateAsync(doctorClinic);

                return ApiResult.Success(ResultCode.Success);
            }
            catch (Exception ex)
            {
                return ApiResult.Fail("系统错误：" + ex.Message, ResultCode.Error);
            }
        }
    }
}


