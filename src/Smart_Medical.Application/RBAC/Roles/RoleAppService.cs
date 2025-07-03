using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smart_Medical.Until;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Microsoft.EntityFrameworkCore;
using Smart_Medical.RBAC; // 引入Domain层的RBAC实体

namespace Smart_Medical.RBAC.Roles
{
    [ApiExplorerSettings(GroupName = "角色管理")]
    public class RoleAppService : ApplicationService, IRoleAppService
    {
        private readonly IRepository<Role, Guid> _roleRepository;

        public RoleAppService(IRepository<Role, Guid> roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<ApiResult> CreateAsync(CreateUpdateRoleDto input)
        {
            var roles = ObjectMapper.Map<CreateUpdateRoleDto, Role>(input);
            var result = await _roleRepository.InsertAsync(roles);
            return ApiResult.Success(ResultCode.Success);
        }

        public async Task<ApiResult> DeleteAsync(Guid id)
        {
            var role = await _roleRepository.GetAsync(id);
            if (role == null)
            {
                return ApiResult.Fail("角色不存在", ResultCode.NotFound);
            }
            await _roleRepository.DeleteAsync(role);
            return ApiResult.Success(ResultCode.Success);
        }

        public async Task<ApiResult<RoleDto>> GetAsync(Guid id)
        {
            // 使用 WithDetailsAsync 联查 UserRoles 和 RolePermissions 实体
            // Ensure that the repository can load navigation properties.
            // WithDetailsAsync is an ABP extension that helps eager load.
            var roleWithDetails = await _roleRepository.WithDetailsAsync(
                r => r.UserRoles.Select(ur => ur.User), // Include UserRole and then the User
                r => r.RolePermissions.Select(rp => rp.Permission) // Include RolePermission and then the Permission
            );

            var result = roleWithDetails.FirstOrDefault(r => r.Id == id);

            if (result == null)
            {
                return ApiResult<RoleDto>.Fail("角色不存在", ResultCode.NotFound);
            }
            var roleDto = ObjectMapper.Map<Role, RoleDto>(result);
            return ApiResult<RoleDto>.Success(roleDto, ResultCode.Success);
        }

        public async Task<ApiResult<PageResult<List<RoleDto>>>> GetListAsync([FromQuery] SeachRoleDto input)
        {
            var queryable = await _roleRepository.GetQueryableAsync();

            // 使用 Include 联查 UserRoles 和 RolePermissions 实体，确保在映射到 DTO 时包含关联数据
            queryable = queryable.Include(r => r.UserRoles).ThenInclude(ur => ur.User) // Include UserRole 及其关联的 User
                                 .Include(r => r.RolePermissions).ThenInclude(rp => rp.Permission); // Include RolePermission 及其关联的 Permission

            if (!string.IsNullOrWhiteSpace(input.RoleName))
            {
                queryable = queryable.Where(r => r.RoleName.Contains(input.RoleName));
            }

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            // Apply paging and sorting
            queryable = queryable
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .OrderBy(r => r.RoleName); // Default sorting

            var roles = await AsyncExecuter.ToListAsync(queryable);
            var roleDtos = ObjectMapper.Map<List<Role>, List<RoleDto>>(roles);

            var pageResult = new PageResult<List<RoleDto>>
            {
                TotleCount = totalCount,
                TotlePage = (int)Math.Ceiling((double)totalCount / input.MaxResultCount),
                Data = roleDtos
            };

            return ApiResult<PageResult<List<RoleDto>>>.Success(pageResult, ResultCode.Success);
        }

        public async Task<ApiResult> UpdateAsync(Guid id, CreateUpdateRoleDto input)
        {
            var role = await _roleRepository.GetAsync(id);
            if (role == null)
            {
                return ApiResult.Fail("角色不存在", ResultCode.NotFound);
            }

            ObjectMapper.Map(input, role);
            await _roleRepository.UpdateAsync(role);
            return ApiResult.Success(ResultCode.Success);
        }
    }
}
