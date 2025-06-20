using Microsoft.AspNetCore.Mvc;
using Smart_Medical.RBAC.Users;
using Smart_Medical.Until;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Smart_Medical.RBAC.Roles
{
    public interface IRoleAppService:IApplicationService
    {
        Task<PageResult<List<RoleDto>>> GetListAsync([FromQuery] Seach seach);
        Task<ApiResult> CreateAsync(CreateUpdateRoleDto input);
    }
}
