using AutoMapper.Internal.Mappers;
using MD5Hash;
using Microsoft.AspNetCore.Mvc;
using Smart_Medical.Until;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Smart_Medical.RBAC; // 引入Domain层的RBAC实体
using Smart_Medical.Application.Contracts.RBAC.Users; // 引用Contracts层的Users DTO和接口
using Smart_Medical.Application.Contracts.RBAC.UserRoles; // 引用Contracts层的UserRoles DTO
using Smart_Medical.Application.Contracts.RBAC.Roles; // 引用Contracts层的Roles DTO

namespace Smart_Medical.RBAC.Users
{
    [ApiExplorerSettings(GroupName = "用户管理")]
    public class UserAppService : ApplicationService, IUserAppService
    {
        private readonly IRepository<User, Guid> _userRepository;

        public UserAppService(IRepository<User, Guid> userRepository)
        {
            _userRepository = userRepository;
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

        public async Task<ApiResult<PageResult<List<UserDto>>>> GetListAsync([FromQuery] SeachUserDto input)
        {
            var queryable = await _userRepository.GetQueryableAsync();

            // 使用 Include 联查 UserRoles 及其关联的 Role 实体，确保在映射到 DTO 时包含关联数据
            queryable = queryable.Include(u => u.UserRoles).ThenInclude(ur => ur.Role);

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

            var users = await AsyncExecuter.ToListAsync(queryable);
            var userDtos = ObjectMapper.Map<List<User>, List<UserDto>>(users);

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
            // 根据用户名查找用户
            var users = await _userRepository.GetQueryableAsync();
            var user = users.FirstOrDefault(u => u.UserName == loginDto.UserName);

            // 检查用户是否存在
            if (user == null)
            {
                return ApiResult<UserDto>.Fail("用户名不存在", ResultCode.NotFound);
            }

            // 验证密码
            if (user.UserPwd != loginDto.UserPwd.GetMD5())
            {
                return ApiResult<UserDto>.Fail("密码错误", ResultCode.ValidationError);
            }

            // 登录成功，返回用户信息
            var userDto = ObjectMapper.Map<User, UserDto>(user);
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
