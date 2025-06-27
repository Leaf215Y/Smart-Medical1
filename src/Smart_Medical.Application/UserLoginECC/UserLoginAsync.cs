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
        //private readonly 
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
        public async Task<ApiResult<ResultLoginDtor>> LoginAsync(LoginDto loginDto)
        {
            // 根据用户名查找用户
            var users = await _userRepository.GetQueryableAsync();
            var user = users.FirstOrDefault(u => u.UserName == loginDto.UserName);

            // 检查用户是否存在
            if (user == null)
            {
                return ApiResult<ResultLoginDtor>.Fail("用户名不存在", ResultCode.NotFound);
            }

            // 验证密码
            if (user.UserPwd != loginDto.UserPwd.GetMD5())
            {
                return ApiResult<ResultLoginDtor>.Fail("密码错误", ResultCode.ValidationError);
            }
           
            // 登录成功，返回用户信息
            //var userDto = ObjectMapper.Map<User, ResultLoginDto>(user);
            var userDtos = ObjectMapper.Map<User,ResultLoginDtor>(user);

            var tokens = await GenerateTokensAsync<User>(user);

            userDtos.AccessToken = tokens.AccessToken;
            userDtos.AccessTokenExpires = tokens.AccessTokenExpires;
            userDtos.RefreshToken = tokens.RefreshToken;
            userDtos.RefreshTokenExpires = tokens.RefreshTokenExpires;

            //userDto.AccessToken = token.CreateJwtToken(user);

            return ApiResult<ResultLoginDtor>.Success(userDtos, ResultCode.Success);
        }


        public async Task<ResultLoginDtor> GenerateTokensAsync<T>(T user)
        {
            LMZTokenHelper token = new LMZTokenHelper(configuration);

            // 1. AccessToken
            var accessToken = token.CreateJwtToken(user);
            var accessTokenExpires = DateTime.UtcNow.AddMinutes(10); // access token 10分钟有效

            // 2. RefreshToken
            var refreshToken = token.GenerateRefreshToken();
            var refreshTokenExpires = DateTime.UtcNow.AddDays(7); // refresh token 7天有效

            // 3. 把 refresh token 存入 Redis（或数据库）
            //await _cache.SetAsync($"RefreshToken:{user.Id}", refreshToken, TimeSpan.FromDays(7));

            // 4. 返回前端
            return new ResultLoginDtor
            {
                AccessToken = accessToken,
                AccessTokenExpires = accessTokenExpires,
                RefreshToken = refreshToken,
                RefreshTokenExpires = refreshTokenExpires
            };
        }

    }
}
