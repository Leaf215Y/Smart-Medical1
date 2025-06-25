using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smart_Medical; // 添加对 ResultCode 命名空间的引用
using Smart_Medical.Application.Contracts.RBAC.Roles; // 引用Contracts层的Roles DTO
using Smart_Medical.Application.Contracts.RBAC.UserRoles; // 引用Contracts层的UserRoles DTO和接口
using Smart_Medical.Application.Contracts.RBAC.Users; // 引用Contracts层的Users DTO
using Smart_Medical.RBAC;
using Smart_Medical.Until;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace Smart_Medical.RBAC.UserRoles
{
    [ApiExplorerSettings(GroupName = "用户角色关联管理")]
    public class UserRoleAppService : ApplicationService, IUserRoleAppService
    {
        private readonly IRepository<UserRole, Guid> _userRoleRepository;
        private readonly IRepository<User, Guid> _userRepository; // 用于获取用户详情
        private readonly IRepository<Role, Guid> _roleRepository; // 用于获取角色详情

        public UserRoleAppService(IRepository<UserRole, Guid> userRoleRepository,
                                  IRepository<User, Guid> userRepository,
                                  IRepository<Role, Guid> roleRepository)
        {
            _userRoleRepository = userRoleRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<ApiResult> CreateAsync(CreateUpdateUserRoleDto input)
        {
            // 检查用户和角色是否存在
            var userExists = await _userRepository.AnyAsync(u => u.Id == input.UserId);
            if (!userExists)
            {
                return ApiResult.Fail("用户不存在", ResultCode.NotFound);
            }
            var roleExists = await _roleRepository.AnyAsync(r => r.Id == input.RoleId);
            if (!roleExists)
            {
                return ApiResult.Fail("角色不存在", ResultCode.NotFound);
            }

            // 检查是否已存在相同的用户角色关联
            var existingUserRole = await _userRoleRepository.FirstOrDefaultAsync(
                ur => ur.UserId == input.UserId && ur.RoleId == input.RoleId
            );
            if (existingUserRole != null)
            {
                return ApiResult.Fail("该用户已拥有此角色", ResultCode.AlreadyExists);
            }

            var userRole = ObjectMapper.Map<CreateUpdateUserRoleDto, UserRole>(input);
            await _userRoleRepository.InsertAsync(userRole);
            return ApiResult.Success(ResultCode.Success);
        }

        public async Task<ApiResult> DeleteAsync(Guid id)
        {
            var userRole = await _userRoleRepository.GetAsync(id);
            if (userRole == null)
            {
                return ApiResult.Fail("用户角色关联不存在", ResultCode.NotFound);
            }
            await _userRoleRepository.DeleteAsync(userRole);
            return ApiResult.Success(ResultCode.Success);
        }

        public async Task<ApiResult<UserRoleDto>> GetAsync(Guid id)
        {
            // 使用 Include 联查 User 和 Role 实体
            var userRole = await _userRoleRepository.WithDetailsAsync(ur => ur.User, ur => ur.Role);

            var result = userRole.FirstOrDefault(ur => ur.Id == id);

            if (result == null)
            {
                return ApiResult<UserRoleDto>.Fail("用户角色关联不存在", ResultCode.NotFound);
            }
            var userRoleDto = ObjectMapper.Map<UserRole, UserRoleDto>(result);
            return ApiResult<UserRoleDto>.Success(userRoleDto, ResultCode.Success);
        }

        public async Task<ApiResult<PageResult<List<UserRoleDto>>>> GetListAsync([FromQuery] SeachUserRoleDto input)
        {
            var queryable = await _userRoleRepository.GetQueryableAsync();

            // 使用 Include 联查 User 和 Role 实体，确保在映射到 DTO 时包含关联数据
            queryable = queryable.Include(ur => ur.User).Include(ur => ur.Role);

            if (input.UserId.HasValue)
            {
                queryable = queryable.Where(ur => ur.UserId == input.UserId.Value);
            }
            if (input.RoleId.HasValue)
            {
                queryable = queryable.Where(ur => ur.RoleId == input.RoleId.Value);
            }

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            queryable = queryable
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .OrderBy(ur => ur.UserId); // 默认排序

            var userRoles = await AsyncExecuter.ToListAsync(queryable);
            var userRoleDtos = ObjectMapper.Map<List<UserRole>, List<UserRoleDto>>(userRoles);

            var pageResult = new PageResult<List<UserRoleDto>>
            {
                TotleCount = totalCount,
                TotlePage = (int)Math.Ceiling((double)totalCount / input.MaxResultCount),
                Data = userRoleDtos
            };

            return ApiResult<PageResult<List<UserRoleDto>>>.Success(pageResult, ResultCode.Success);
        }
        
        [UnitOfWork] // 添加 [UnitOfWork] 特性，确保此方法的数据库操作在事务中执行
        public async Task<ApiResult> UpdateAsync(Guid userId, List<Guid> roleIds)
        {
            // 1. 验证用户是否存在
            var userExists = await _userRepository.AnyAsync(u => u.Id == userId);
            if (!userExists)
            {
                return ApiResult.Fail("用户不存在", ResultCode.NotFound);
            }

            // 2. 验证所有传入的角色ID是否存在
            foreach (var roleId in roleIds)
            {
                var roleExists = await _roleRepository.AnyAsync(r => r.Id == roleId);
                if (!roleExists)
                {
                    return ApiResult.Fail($"角色ID {roleId} 不存在", ResultCode.NotFound);
                }
            }

            // 获取用户当前的角色关联 (只包含非删除的)
            var activeUserRoles = (await _userRoleRepository.GetQueryableAsync()).Where(ur => ur.UserId == userId).ToList();
            var activeRoleIds = activeUserRoles.Select(ur => ur.RoleId).ToList();

            // 获取所有（包括软删除）该用户的角色关联
            var queryableAllUserRoles = await _userRoleRepository.GetQueryableAsync();
            var allUserRolesList = await queryableAllUserRoles.AsNoTracking().IgnoreQueryFilters()
                                                                .Where(ur => ur.UserId == userId)
                                                                .ToListAsync();

            // 需要软删除的角色：当前活跃，但新传入的roleIds中没有的
            var rolesToSoftDelete = activeRoleIds.Except(roleIds).ToList();

            // 需要新增或恢复的角色：新传入的roleIds中，但当前不活跃的
            var rolesToCreateOrRestore = roleIds.Except(activeRoleIds).ToList();

            // 执行软删除操作
            foreach (var roleId in rolesToSoftDelete)
            {
                var userRoleToSoftDelete = activeUserRoles.FirstOrDefault(ur => ur.RoleId == roleId);
                if (userRoleToSoftDelete != null)
                {
                    await _userRoleRepository.DeleteAsync(userRoleToSoftDelete); // This performs soft delete
                }
            }

            // 执行新增或恢复操作
            foreach (var roleId in rolesToCreateOrRestore)
            {
                // 查找是否存在已存在的（包括软删除的）记录
                var existingUserRole = allUserRolesList.FirstOrDefault(ur => ur.RoleId == roleId);

                if (existingUserRole != null)
                {
                    // 如果存在且是软删除状态，则恢复
                    if (existingUserRole.IsDeleted) // Assuming UserRole implements ISoftDelete
                    {
                        existingUserRole.IsDeleted = false;
                        existingUserRole.DeletionTime = null;
                        await _userRoleRepository.UpdateAsync(existingUserRole);
                    }
                    // 如果存在且不是软删除状态，则不做任何操作 (理论上不应该进入这个分支，因为 rolesToCreateOrRestore 已经排除了活跃的)
                }
                else
                {
                    // 不存在任何记录（包括软删除），则新增
                    await _userRoleRepository.InsertAsync(new UserRole(GuidGenerator.Create()) { UserId = userId, RoleId = roleId });
                }
            }

            return ApiResult.Success(ResultCode.Success);
        }
    }
}