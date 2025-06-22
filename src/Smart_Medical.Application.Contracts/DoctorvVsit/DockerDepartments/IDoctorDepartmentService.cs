using Smart_Medical.Until;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Smart_Medical.DoctorvVsit.DockerDepartments
{
    public interface IDoctorDepartmentService:IApplicationService
    {
        Task<ApiResult> InsertDoctorDepartment(CreateUpdateDoctorDepartmentDto input);
    }
}
