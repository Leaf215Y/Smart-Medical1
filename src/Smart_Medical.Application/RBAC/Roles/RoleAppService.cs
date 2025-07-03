using Microsoft.AspNetCore.Mvc;
using Smart_Medical.Application.Contracts.RBAC.Roles; // 引入 Contracts 层的角色 DTO 和接口
using Smart_Medical.Until;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Microsoft.EntityFrameworkCore;
using Smart_Medical.RBAC; // 引入Domain层的RBAC实体

namespace Smart_Medical.RBAC.Roles
{
    /// <summary>
    /// 角色管理服务
    /// </summary>
    [ApiExplorerSettings(GroupName = "角色管理")]
    [Dependency(ReplaceServices = true)]
    public class RoleAppService : ApplicationService, IRoleAppService
    {
        private readonly IRepository<Role, Guid> _roleRepository;

        public RoleAppService(IRepository<Role, Guid> roleRepository)
        {
            _roleRepository = roleRepository;
        }

        /// <summary>
        /// 创建一个新角色
        /// </summary>
        /// <param name="input">创建角色所需的数据传输对象</param>
        /// <returns>操作结果</returns>
        public async Task<ApiResult> CreateAsync(CreateUpdateRoleDto input)
        {
            var roles = ObjectMapper.Map<CreateUpdateRoleDto, Role>(input);
            var result = await _roleRepository.InsertAsync(roles);
            return ApiResult.Success(ResultCode.Success);
        }

        /// <summary>
        /// 根据ID删除一个角色
        /// </summary>
        /// <param name="id">要删除的角色ID</param>
        /// <returns>操作结果</returns>
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

        /// <summary>
        /// 根据ID获取单个角色的详细信息
        /// </summary>
        /// <param name="id">要查询的角色ID</param>
        /// <returns>包含角色详细信息的ApiResult</returns>
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

        /// <summary>
        /// 根据查询条件分页获取角色列表
        /// </summary>
        /// <param name="input">包含分页和筛选信息的查询DTO</param>
        /// <returns>包含角色列表和分页信息的ApiResult</returns>
        public async Task<ApiResult<PageResult<List<RoleDto>>>> GetListAsync([FromQuery] SeachRoleDto input)
        {
            var queryable = await _roleRepository.GetQueryableAsync();

            // 使用投影（Select）时，只有在投影的字段中包含导航属性时才需要Include。
            // RoleDto不包含关联集合，因此此处可以移除Include，以提升性能。
            // queryable = queryable.Include(r => r.UserRoles).ThenInclude(ur => ur.User) 
            //                      .Include(r => r.RolePermissions).ThenInclude(rp => rp.Permission); 

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

            var roleDtos = await AsyncExecuter.ToListAsync(
                queryable.Select(r => new RoleDto
                {
                    // 【诊断步骤】暂时只选择非Guid字段，排查Guid解析错误
                     Id = r.Id,
                    RoleName = r.RoleName,
                    Description = r.Description,
                    CreationTime = r.CreationTime,
                     //CreatorId = r.CreatorId,
                    // LastModificationTime = r.LastModificationTime,
                     LastModifierId = r.LastModifierId
                })
            );

            var pageResult = new PageResult<List<RoleDto>>
            {
                TotleCount = totalCount,
                TotlePage = (int)Math.Ceiling((double)totalCount / input.MaxResultCount),
                Data = roleDtos
            };

            return ApiResult<PageResult<List<RoleDto>>>.Success(pageResult, ResultCode.Success);
        }

        /// <summary>
        /// 根据ID更新一个已有的角色
        /// </summary>
        /// <param name="id">要更新的角色ID</param>
        /// <param name="input">包含新角色信息的数据传输对象</param>
        /// <returns>操作结果</returns>
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
