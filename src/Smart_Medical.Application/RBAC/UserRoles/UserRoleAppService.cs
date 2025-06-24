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
using Smart_Medical.Application.Contracts.RBAC.UserRoles; // 引用Contracts层的UserRoles DTO和接口
using Smart_Medical.Application.Contracts.RBAC.Users; // 引用Contracts层的Users DTO
using Smart_Medical.Application.Contracts.RBAC.Roles; // 引用Contracts层的Roles DTO
using Smart_Medical; // 添加对 ResultCode 命名空间的引用

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

        // UpdateAsync 方法对于中间表通常不需要，因为关联的修改通常是删除旧关联再创建新关联。
        // 如果需要修改 UserId 或 RoleId，通常会先删除再新增。
        // 此处省略 UpdateAsync，如果确实需要，可以根据业务逻辑添加。
        public Task<ApiResult> UpdateAsync(Guid id, CreateUpdateUserRoleDto input)
        {
            // 对于多对多中间表，通常通过删除旧记录和插入新记录来"更新"关联。
            // 此处不实现具体的 Update 逻辑，因为其语义不如 Delete + Create 清晰。
            throw new NotImplementedException("更新用户角色关联通常通过删除和重新创建实现。");
        }
    }
}