using Smart_Medical.DoctorvVsit;
using Smart_Medical.Medical;
using Smart_Medical.OutpatientClinic.Dtos;
using Smart_Medical.OutpatientClinic.IServices;
using Smart_Medical.Patient;
using Smart_Medical.Until;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
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
    public async Task<ApiResult> RegistrationPatientAsync(InsertPatientDto input)
    {
        using (var uow = _unitOfWorkManager.Begin(requiresNew: true))
        {
            try
            {
                // 1. 创建患者基本信息
                BasicPatientInfo patient = ObjectMapper.Map<InsertPatientDto, BasicPatientInfo>(input);
                patient.VisitStatus = "待就诊";
                var Ispatientc = await _patientRepo.InsertAsync(patient) != null ? true : false;
                if (!Ispatientc)
                {
                    return ApiResult.Fail("患者信息登记失败，请稍后重试！", ResultCode.Error);
                }
                // 2. 创建就诊流程
                DoctorClinic doctorClinic = ObjectMapper.Map<InsertPatientDto, DoctorClinic>(input);
                // 关联患者ID
                doctorClinic.PatientId = patient.Id;
                doctorClinic.DispensingStatus = 0;

                var IsClinic = await _doctorclinRepo.InsertAsync(doctorClinic) != null ? true : false;
                if(!IsClinic)
                {
                    return ApiResult.Fail("就诊流程创建失败，请稍后重试！", ResultCode.Error);
                }
                // 3. 创建患者病历信息
                Sick sick = ObjectMapper.Map<InsertPatientDto, Sick>(input);
                sick.AdmissionDiagnosis = "";
                sick.DischargeDepartment = "";
                // 关联患者ID
                sick.DischargeDiagnosis = patient.Id.ToString();
                sick.InpatientNumber = "";
                sick.Status = "";
                sick.Name = patient.PatientName;
                var IsSick = await _sickRepo.InsertAsync(sick) != null ? true : false; 
                if(!IsSick)
                {
                    return ApiResult.Fail("患者病历信息创建失败，请稍后重试！", ResultCode.Error);
                }

                // 👈 提交事务
                await uow.CompleteAsync();

                return ApiResult.Success(ResultCode.Success);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
