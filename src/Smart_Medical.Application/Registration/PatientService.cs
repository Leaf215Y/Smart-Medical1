using Smart_Medical.DoctorvVsit;
using Smart_Medical.Medical;
using Smart_Medical.OutpatientClinic.Dtos;
using Smart_Medical.OutpatientClinic.Dtos.Parameter;
using Smart_Medical.OutpatientClinic.IServices;
using Smart_Medical.Patient;
using Smart_Medical.Until;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
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

    public PatientService(
        IUnitOfWorkManager unitOfWorkManager, IRepository<DoctorClinic, Guid> doctorclinRepo, IRepository<BasicPatientInfo, Guid> basicpatientRepo, IRepository<Sick, Guid> sickRepo)
    {
        _unitOfWorkManager = unitOfWorkManager;
        _doctorclinRepo = doctorclinRepo;
        _patientRepo = basicpatientRepo;
        _sickRepo = sickRepo;
    }

    /// <summary>
    /// 快速就诊、登记患者信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<ApiResult> RegistrationPatientAsync(InsertPatientDto input)
    {
        using (var uow = _unitOfWorkManager.Begin(requiresNew: true))
        {
            try
            {
                // ================================
                //  1. 创建患者基本信息
                // ================================
                BasicPatientInfo patient = ObjectMapper.Map<InsertPatientDto, BasicPatientInfo>(input);
                patient.VisitStatus = "待就诊"; // 初始状态设为“待就诊”

                var isPatientInserted = await _patientRepo.InsertAsync(patient) != null;
                if (!isPatientInserted)
                {
                    return ApiResult.Fail("患者信息登记失败，请稍后重试！", ResultCode.Error);
                }

                // ================================
                //  2. 创建就诊流程记录（排队 + 接诊）
                // ================================
                DoctorClinic doctorClinic = ObjectMapper.Map<InsertPatientDto, DoctorClinic>(input);
                doctorClinic.PatientId = patient.Id;         // 关联患者 ID
                doctorClinic.DispensingStatus = 0;           // 默认“未发药”

                var isClinicInserted = await _doctorclinRepo.InsertAsync(doctorClinic) != null;
                if (!isClinicInserted)
                {
                    return ApiResult.Fail("就诊流程创建失败，请稍后重试！", ResultCode.Error);
                }

                // ================================
                // 3. 创建患者病历信息（暂为空）
                // ================================
                Sick sick = ObjectMapper.Map<InsertPatientDto, Sick>(input);
                sick.AdmissionDiagnosis = "";                      // 初始为空
                sick.DischargeDepartment = "";                     // 初始为空
                sick.DischargeDiagnosis = patient.Id.ToString();   // 绑定患者 ID（注意字段命名可能有点奇怪）
                sick.InpatientNumber = "";                         // 暂无住院号
                sick.Status = "";                                  // 暂无状态
                sick.Name = patient.PatientName;                   // 冗余存下姓名方便查询

                var isSickInserted = await _sickRepo.InsertAsync(sick) != null;
                if (!isSickInserted)
                {
                    return ApiResult.Fail("患者病历信息创建失败，请稍后重试！", ResultCode.Error);
                }

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

    //就诊患者详细信息

}
