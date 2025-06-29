using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Smart_Medical.Until;
using Volo.Abp.Application.Dtos;
using Smart_Medical.RBAC.Permissions;

namespace Smart_Medical.Application.Contracts.RBAC.Permissions
{
    /// <summary>
    /// 权限接口服务
    /// </summary>
    public interface IPermissionAppService : IApplicationService
    {
        Task<ApiResult<PermissionDto>> GetAsync(Guid id);

        Task<ApiResult<PageResult<List<PermissionDto>>>> GetListAsync(SeachPermissionDto input);

        Task<ApiResult> CreateAsync(CreateUpdatePermissionDto input);

        Task<ApiResult> UpdateAsync(Guid id, CreateUpdatePermissionDto input);

        Task<ApiResult> DeleteAsync(Guid id);
        Task<ApiResult<List<GetMenuPermissionTree>>> GetMenuPermissionTreeList(Guid? parentId=null);
    }
}