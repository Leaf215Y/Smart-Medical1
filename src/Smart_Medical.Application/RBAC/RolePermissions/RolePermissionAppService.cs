using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Smart_Medical.Until;
using Smart_Medical.RBAC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smart_Medical.Application.Contracts.RBAC.RolePermissions; // 引用Contracts层的RolePermissions DTO和接口
using Smart_Medical.Application.Contracts.RBAC.Roles; // 引用Contracts层的Roles DTO
using Smart_Medical.Application.Contracts.RBAC.Permissions; // 引用Contracts层的Permissions DTO
using Smart_Medical; // 添加对 ResultCode 命名空间的引用

namespace Smart_Medical.RBAC.RolePermissions
{
    [ApiExplorerSettings(GroupName = "角色权限关联管理")]
    public class RolePermissionAppService : ApplicationService, IRolePermissionAppService
    {
        private readonly IRepository<RolePermission, Guid> _rolePermissionRepository;
        private readonly IRepository<Role, Guid> _roleRepository; // 用于获取角色详情
        private readonly IRepository<Permission, Guid> _permissionRepository; // 用于获取权限详情

        public RolePermissionAppService(IRepository<RolePermission, Guid> rolePermissionRepository,
                                        IRepository<Role, Guid> roleRepository,
                                        IRepository<Permission, Guid> permissionRepository)
        {
            _rolePermissionRepository = rolePermissionRepository;
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
        }

        public async Task<ApiResult> CreateAsync(CreateUpdateRolePermissionDto input)
        {
            // 检查角色和权限是否存在
            var roleExists = await _roleRepository.AnyAsync(r => r.Id == input.RoleId);
            if (!roleExists)
            {
                return ApiResult.Fail("角色不存在", ResultCode.NotFound);
            }
            var permissionExists = await _permissionRepository.AnyAsync(p => p.Id == input.PermissionId);
            if (!permissionExists)
            {
                return ApiResult.Fail("权限不存在", ResultCode.NotFound);
            }

            // 检查是否已存在相同的角色权限关联
            var existingRolePermission = await _rolePermissionRepository.FirstOrDefaultAsync(
                rp => rp.RoleId == input.RoleId && rp.PermissionId == input.PermissionId
            );
            if (existingRolePermission != null)
            {
                return ApiResult.Fail("该角色已拥有此权限", ResultCode.AlreadyExists);
            }

            var rolePermission = ObjectMapper.Map<CreateUpdateRolePermissionDto, RolePermission>(input);
            await _rolePermissionRepository.InsertAsync(rolePermission);
            return ApiResult.Success(ResultCode.Success);
        }

        public async Task<ApiResult> DeleteAsync(Guid id)
        {
            var rolePermission = await _rolePermissionRepository.GetAsync(id);
            if (rolePermission == null)
            {
                return ApiResult.Fail("角色权限关联不存在", ResultCode.NotFound);
            }
            await _rolePermissionRepository.DeleteAsync(rolePermission);
            return ApiResult.Success(ResultCode.Success);
        }

        public async Task<ApiResult<RolePermissionDto>> GetAsync(Guid id)
        {
            // 使用 Include 联查 Role 和 Permission 实体
            var rolePermission = await _rolePermissionRepository.WithDetailsAsync(rp => rp.Role, rp => rp.Permission);

            var result = rolePermission.FirstOrDefault(rp => rp.Id == id);

            if (result == null)
            {
                return ApiResult<RolePermissionDto>.Fail("角色权限关联不存在", ResultCode.NotFound);
            }
            var rolePermissionDto = ObjectMapper.Map<RolePermission, RolePermissionDto>(result);
            return ApiResult<RolePermissionDto>.Success(rolePermissionDto, ResultCode.Success);
        }

        public async Task<ApiResult<PageResult<List<RolePermissionDto>>>> GetListAsync([FromQuery] SeachRolePermissionDto input)
        {
            var queryable = await _rolePermissionRepository.GetQueryableAsync();

            // 使用 Include 联查 Role 和 Permission 实体，确保在映射到 DTO 时包含关联数据
            queryable = queryable.Include(rp => rp.Role).Include(rp => rp.Permission);

            if (input.RoleId.HasValue)
            {
                queryable = queryable.Where(rp => rp.RoleId == input.RoleId.Value);
            }
            if (input.PermissionId.HasValue)
            {
                queryable = queryable.Where(rp => rp.PermissionId == input.PermissionId.Value);
            }

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            queryable = queryable
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .OrderBy(rp => rp.RoleId); // 默认排序

            var rolePermissions = await AsyncExecuter.ToListAsync(queryable);
            var rolePermissionDtos = ObjectMapper.Map<List<RolePermission>, List<RolePermissionDto>>(rolePermissions);

            var pageResult = new PageResult<List<RolePermissionDto>>
            {
                TotleCount = totalCount,
                TotlePage = (int)Math.Ceiling((double)totalCount / input.MaxResultCount),
                Data = rolePermissionDtos
            };

            return ApiResult<PageResult<List<RolePermissionDto>>>.Success(pageResult, ResultCode.Success);
        }

        // UpdateAsync 方法对于中间表通常不需要，因为关联的修改通常是删除旧关联再创建新关联。
        // 如果需要修改 RoleId 或 PermissionId，通常会先删除再新增。
        // 此处省略 UpdateAsync，如果确实需要，可以根据业务逻辑添加。
        public Task<ApiResult> UpdateAsync(Guid id, CreateUpdateRolePermissionDto input)
        {
            // 对于多对多中间表，通常通过删除旧记录和插入新记录来"更新"关联。
            // 此处不实现具体的 Update 逻辑，因为其语义不如 Delete + Create 清晰。
            throw new NotImplementedException("更新角色权限关联通常通过删除和重新创建实现。");
        }
    }
}