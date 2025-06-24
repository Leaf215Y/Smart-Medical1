using Smart_Medical.OutpatientClinic.Dtos;
using Smart_Medical.OutpatientClinic.Dtos.Parameter;
using Smart_Medical.Until;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Smart_Medical.OutpatientClinic.IServices
{
    public interface IPatientService : IApplicationService
    {
        /// <summary>
        /// 登记患者信息
        /// </summary>
        /// <returns></returns>
        Task<ApiResult> RegistrationPatientAsync(InsertPatientDto input);

        /// <summary>
        /// 就诊患者
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<PagedResultDto<GetVisitingDto>>> VisitingPatientsAsync(GetVistingParameterDtos input);

        /// <summary>
        /// 就诊患者详细信息
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        Task<ApiResult<BasicPatientInfoDto>> GetPatientInfoAsync(Guid patientId);

        /// <summary>
        /// 患者所有病历信息
        /// </summary>
        /// <param name="patientId">病历外键</param>
        /// <returns></returns>
        Task<ApiResult<List<GetSickInfoDto>>> GetPatientSickInfoAsync(Guid patientId);

        /// <summary>
        /// 开具处方
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ApiResult> DoctorsPrescription(DoctorPrescriptionDto input);
    }
}
