using MD5Hash;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smart_Medical.Until;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Smart_Medical.RBAC; // 引入Domain层的RBAC实体
using Smart_Medical.Until.Redis;
using Smart_Medical.RBAC.Permissions;

namespace Smart_Medical.RBAC.Users
{
    [ApiExplorerSettings(GroupName = "用户管理")]
    public class UserAppService : ApplicationService, IUserAppService
    {
        // 定义一个常量作为缓存键，这是这个特定缓存项在 Redis 中的唯一标识。
        // 使用一个清晰且唯一的键很重要。
        private const string CacheKey = "permission:All"; // 建议使用更具体的键名和前缀
        private readonly IRepository<User, Guid> _userRepository;
        private readonly IRedisHelper<List<PermissionDto>> redisHelper;
        private readonly IRepository<Permission, Guid> permission;

        public UserAppService(IRepository<User, Guid> userRepository, IRedisHelper<List<PermissionDto>> redisHelper,IRepository<Permission,Guid> permission)
        {
            _userRepository = userRepository;
            this.redisHelper = redisHelper;
            this.permission = permission;
        }

        public async Task<ApiResult> CreateAsync(CreateUpdateUserDto input)
        {
            input.UserPwd = input.UserPwd.GetMD5();
            var user = ObjectMapper.Map<CreateUpdateUserDto, User>(input);
            var result = await _userRepository.InsertAsync(user);
            return ApiResult.Success(ResultCode.Success);
        }

        public async Task<ApiResult> DeleteAsync(Guid id)
        {
            var user = await _userRepository.GetAsync(id);
            if (user == null)
            {
                return ApiResult.Fail("用户不存在", ResultCode.NotFound);
            }
            await _userRepository.DeleteAsync(user);
            return ApiResult.Success(ResultCode.Success);
        }
       
        public async Task<ApiResult<UserDto>> GetAsync(Guid id)
        {
            // 使用 WithDetailsAsync 联查 UserRoles 及其关联的 Role 实体
            var user = await _userRepository.WithDetailsAsync(u => u.UserRoles.Select(ur => ur.Role));

            var result = user.FirstOrDefault(u => u.Id == id);

            if (result == null)
            {
                return ApiResult<UserDto>.Fail("用户不存在", ResultCode.NotFound);
            }
            var userDto = ObjectMapper.Map<User, UserDto>(result);
            return ApiResult<UserDto>.Success(userDto, ResultCode.Success);
        }

        /// <summary>
        /// 根据查询条件分页获取用户列表
        /// </summary>
        /// <param name="input">包含分页和筛选信息的查询DTO</param>
        /// <returns>包含用户列表和分页信息的ApiResult</returns>
        public async Task<ApiResult<PageResult<List<UserDto>>>> GetListAsync([FromQuery] SeachUserDto input)
        {
            var queryable = await _userRepository.GetQueryableAsync();

            // BUG修复：为了能在后续投影中安全地获取角色名，需要Include关联的角色数据
            queryable = queryable
                .Include(u => u.UserRoles).ThenInclude(ur => ur.Role);

            if (!string.IsNullOrWhiteSpace(input.UserName))
            {
                queryable = queryable.Where(u => u.UserName.Contains(input.UserName));
            }
            if (!string.IsNullOrWhiteSpace(input.UserEmail))
            {
                queryable = queryable.Where(u => u.UserEmail.Contains(input.UserEmail));
            }
            if (!string.IsNullOrWhiteSpace(input.UserPhone))
            {
                queryable = queryable.Where(u => u.UserPhone.Contains(input.UserPhone));
            }

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            queryable = queryable
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .OrderBy(u => u.UserName); // 默认排序

            // 使用 Select 进行投影，直接生成 UserDto 列表，绕过 AutoMapper
            var userDtos = await AsyncExecuter.ToListAsync(
                queryable.Select(u => new UserDto
                {
                    UserName = u.UserName,
                    UserEmail = u.UserEmail,
                    UserPhone = u.UserPhone,
                    UserSex = u.UserSex,
                    // EF Core表达式树不支持空传播运算符(?.), 改用Select().FirstOrDefault()达到同样的安全效果
                    RoleName = u.UserRoles.Select(ur => ur.Role.RoleName).FirstOrDefault()

                    // 根据你的要求，不再映射审计字段
                })
            );

            var pageResult = new PageResult<List<UserDto>>
            {
                TotleCount = totalCount,
                TotlePage = (int)Math.Ceiling((double)totalCount / input.MaxResultCount),
                Data = userDtos
            };

            return ApiResult<PageResult<List<UserDto>>>.Success(pageResult, ResultCode.Success);
        }

        public async Task<ApiResult<UserDto>> GetByUserNameAsync(string username)
        {
            var user = await _userRepository.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                return ApiResult<UserDto>.Fail("用户不存在", ResultCode.NotFound);
            }
            var userDto = ObjectMapper.Map<User, UserDto>(user);
            return ApiResult<UserDto>.Success(userDto, ResultCode.Success);
        }

        public async Task<ApiResult<UserDto>> LoginAsync(LoginDto loginDto)
        {
            // 1. 根据用户名查找用户
            var users = await _userRepository.GetQueryableAsync();
            //// 联查用户-角色-权限
            //var user = await users
            //    .Include(u => u.UserRoles)
            //        .ThenInclude(ur => ur.Role)
            //            .ThenInclude(r => r.RolePermissions)
            //                .ThenInclude(rp => rp.Permission)
            //    .FirstOrDefaultAsync(u => u.UserName == loginDto.UserName);
            var user= await users.FirstOrDefaultAsync(u => u.UserName == loginDto.UserName);

            // 2. 检查用户是否存在
            if (user == null)
            {
                return ApiResult<UserDto>.Fail("用户名不存在", ResultCode.NotFound);
            }

            // 3. 验证密码
            if (user.UserPwd != loginDto.UserPwd.GetMD5())
            {
                return ApiResult<UserDto>.Fail("密码错误", ResultCode.ValidationError);
            }

            // 4. 登录成功，组装用户信息
            var userDto = ObjectMapper.Map<User, UserDto>(user);

            // 5. 角色列表
            //userDto.Roles = user.UserRoles?
            //    .Select(ur => ur.Role?.RoleName)
            //    .Where(rn => !string.IsNullOrEmpty(rn))
            //    .Distinct()
            //    .ToList() ?? new List<string>();

            //// 6. 权限列表（去重，返回权限编码或名称均可）
            //userDto.Permissions = user.UserRoles?
            //    .SelectMany(ur => ur.Role?.RolePermissions ?? new List<RolePermission>())
            //    .Select(rp => rp.Permission?.PermissionCode)
            //    .Where(pc => !string.IsNullOrEmpty(pc))
            //    .Distinct()
            //    .ToList() ?? new List<string>();
            var permissiondtos= await permission.GetQueryableAsync();
            permissiondtos= permissiondtos.Where(x=>x.Type==Enums.PermissionType.Button);
            userDto.Permissions = permissiondtos.Select(x=>x.PermissionCode).ToList();

            return ApiResult<UserDto>.Success(userDto, ResultCode.Success);
        }

        public async Task<ApiResult> UpdateAsync(Guid id, CreateUpdateUserDto input)
        {
            var user = await _userRepository.GetAsync(id);
            if (user == null)
            {
                return ApiResult.Fail("用户不存在", ResultCode.NotFound);
            }

            ObjectMapper.Map(input, user);
            user.UserPwd = input.UserPwd.GetMD5(); // 确保密码被加密
            await _userRepository.UpdateAsync(user);
            return ApiResult.Success(ResultCode.Success);
        }
    }
}
