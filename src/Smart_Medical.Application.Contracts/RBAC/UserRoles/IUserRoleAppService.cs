using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Application.Dtos;
using Smart_Medical.Until;

namespace Smart_Medical.Application.Contracts.RBAC.UserRoles
{
    /// <summary>
    /// 用户角色关联接口服务
    /// </summary>
    public interface IUserRoleAppService : IApplicationService
    {
        Task<ApiResult<UserRoleDto>> GetAsync(Guid id);

        Task<ApiResult<PageResult<List<UserRoleDto>>>> GetListAsync(SeachUserRoleDto input);

        Task<ApiResult> CreateAsync(CreateUpdateUserRoleDto input);

        Task<ApiResult> DeleteAsync(Guid id);

        Task<ApiResult> UpdateAsync(Guid id, CreateUpdateUserRoleDto input);
    }
}