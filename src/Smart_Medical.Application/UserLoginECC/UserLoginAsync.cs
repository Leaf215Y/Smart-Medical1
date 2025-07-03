using AutoMapper.Internal.Mappers;
using MD5Hash;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Smart_Medical.Dictionarys;
using Smart_Medical.Dictionarys.DictionaryTypes;
using Smart_Medical.RBAC;
using Smart_Medical.RBAC.Users;
using Smart_Medical.Until;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Security.Claims;

namespace Smart_Medical.UserLoginECC
{
    /// <summary>
    /// 用户登录
    /// </summary>
    [ApiExplorerSettings(GroupName = "用户登录")]
    public class UserLoginAsync : ApplicationService , IUserLoginAsyncService
    {

        private readonly IDistributedCache<TokenPairDto> _refreshTokenCache;
        private readonly LMZTokenHelper _tokenHelper;
        private readonly IRepository<User, Guid> _userRepository;
        private readonly IConfiguration _configuration;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public UserLoginAsync(
            IRepository<User, Guid> userRepository, 
            IConfiguration configuration, 
            IDistributedCache<TokenPairDto> refreshTokenCache, 
            LMZTokenHelper tokenHelper)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _refreshTokenCache = refreshTokenCache;
            _tokenHelper = tokenHelper;
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        public async Task<ApiResult<ResultLoginDto>> LoginAsync(LoginDto loginDto)
        {
            try
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

                var tokens = await GenerateTokensAsync(user);

                userDto.AccessToken = tokens.AccessToken;
                userDto.RefreshToken = tokens.RefreshToken;

                return ApiResult<ResultLoginDto>.Success(userDto, ResultCode.Success);
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 生成双token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>

        public async Task<TokenPairDtos> GenerateTokensAsync(User user)
        {
            LMZTokenHelper token = new LMZTokenHelper(_configuration);

            // 1. AccessToken
            var accessToken = token.CreateJwtToken(user);
            var accessTokenExpires = DateTime.UtcNow.AddMinutes(1); //10分钟有效

            // 2. RefreshToken
            var refreshToken = token.CreateJwtToken(user,30);
            var refreshTokenExpires = DateTime.UtcNow.AddMinutes(30);

            var cacheKey = $"SmartMedical:RefreshToken{user.Id}";

            var cacheItem = new TokenPairDto
            {
                UserNumber = user.Id,
                RefreshToken = refreshToken,
                RefreshTokenExpires = refreshTokenExpires
            };

            //redis中的过期时间
            var expiresIn = refreshTokenExpires - DateTime.UtcNow;

            await _refreshTokenCache.SetAsync(
                cacheKey,
                cacheItem,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiresIn
                }
            );

            // 4. 返回前端
            return new TokenPairDtos
            {
                AccessToken = accessToken,
                AccessTokenExpires = accessTokenExpires,
                RefreshToken = refreshToken,
                RefreshTokenExpires = refreshTokenExpires
            };
        }

        /// <summary>
        /// 刷新令牌接口
        /// </summary>
        /// <param name="input">包含旧的 RefreshToken</param>
        /// <returns>新的 AccessToken 和 RefreshToken</returns>
        [HttpPost] // 通过 HTTP Post 访问
        [Route("refresh")] // 路由会是 /api/app/authentication/refresh (默认路由前缀)
        [AllowAnonymous] // 刷新令牌接口通常不需要 AccessToken 授权，所以允许匿名访问
        public async Task<ResultLoginDto> RefreshTokenAsync(RefreshTokenRequestDto input)
        {
            if (string.IsNullOrWhiteSpace(input.RefreshToken))
            {
                throw new UserFriendlyException("刷新令牌不能为空", "400");
            }

            Guid userId;
            ClaimsPrincipal principal; // 声明 principal 为 ClaimsPrincipal 类型
            try
            {
                principal = _tokenHelper.GetPrincipalFromRefreshToken(input.RefreshToken);

                var userIdClaim = principal?.Claims.FirstOrDefault(c => c.Type == "Id"); // <-- 根据截图，这里用 "Id" 字符串

                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out userId))
                {
                    // 如果找不到用户ID的声明，或者解析失败，说明令牌有问题
                    throw new UserFriendlyException("刷新令牌无效或无法解析用户ID", "401");
                }
            }
            catch (SecurityTokenExpiredException) // 专门捕获 RefreshToken 过期异常
            {
                throw new UserFriendlyException("刷新令牌已过期，请重新登录", "401");
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "解析刷新令牌失败: {RefreshToken}", input.RefreshToken);
                throw new UserFriendlyException("刷新令牌无效或签名验证失败，请重新登录", "401");
            }
                                                         
            var user = await _userRepository.GetAsync(userId);
            if (user == null)
            {
                throw new UserFriendlyException("用户不存在", "404");
            }

            // 6. 生成新的令牌对 (AccessToken 和新的 RefreshToken)
            var newTokens = await GenerateTokensAsync(user); // GenerateTokensAsync 内部会存储新的 RefreshToken

            // 7. 返回新的令牌信息
            var resultDto = ObjectMapper.Map<User, ResultLoginDto>(user);
            resultDto.AccessToken = newTokens.AccessToken;
            resultDto.RefreshToken = newTokens.RefreshToken;
            resultDto.AccessTokenExpires = newTokens.AccessTokenExpires;
            resultDto.RefreshTokenExpires = newTokens.RefreshTokenExpires;

            return resultDto;
        }
    }
}
