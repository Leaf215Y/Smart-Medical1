using Microsoft.AspNetCore.Mvc;
using Smart_Medical.Until;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Smart_Medical.DoctorvVsit.DockerDepartments
{
    /// <summary>
    /// 医生科室管理服务接口
    /// </summary>
    public interface IDoctorDepartmentService : IApplicationService
    {
        Task<ApiResult> InsertDoctorDepartment(CreateUpdateDoctorDepartmentDto input);
        Task<ApiResult<PageResult<List<GetDoctorDepartmentListDto>>>> GetDoctorDepartmentList([FromQuery] GetDoctorDepartmentSearchDto search);
        Task<ApiResult> UpdateDoctorDepartment(Guid id, CreateUpdateDoctorDepartmentDto input);
        Task<ApiResult> DeleteDoctorDepartment(string idsString);
    }
}
