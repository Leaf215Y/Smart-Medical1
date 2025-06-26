using AutoMapper.Internal.Mappers;
using MD5Hash;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Smart_Medical.Application.Contracts.RBAC.Users;
using Smart_Medical.RBAC;
using Smart_Medical.Until;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Smart_Medical.UserLoginECC
{
    /// <summary>
    /// 用户登录
    /// </summary>
    [ApiExplorerSettings(GroupName = "用户登录")]
    public class UserLoginAsync : ApplicationService , IUserLoginAsyncService
    {
        private readonly IRepository<User, Guid> _userRepository;
        private readonly IConfiguration configuration;

        public UserLoginAsync(IRepository<User, Guid> userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            this.configuration = configuration;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        public async Task<ApiResult<ResultLoginDto>> LoginAsync(LoginDto loginDto)
        {
            // 根据用户名查找用户
            var users = await _userRepository.GetQueryableAsync();
            var user = users.FirstOrDefault(u => u.UserName == loginDto.UserName);

            // 检查用户是否存在
            if (user == null)
            {
                return ApiResult<ResultLoginDto>.Fail("用户名不存在", ResultCode.NotFound);
            }

            // 验证密码
            if (user.UserPwd != loginDto.UserPwd.GetMD5())
            {
                return ApiResult<ResultLoginDto>.Fail("密码错误", ResultCode.ValidationError);
            }

            

            // 登录成功，返回用户信息
            var userDto = ObjectMapper.Map<User, ResultLoginDto>(user);
            return ApiResult<ResultLoginDto>.Success(userDto, ResultCode.Success);
        }

        
    }
}
