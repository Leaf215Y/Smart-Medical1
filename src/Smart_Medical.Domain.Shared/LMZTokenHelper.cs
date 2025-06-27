using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
namespace Smart_Medical
{
    public class LMZTokenHelper
    {
        private readonly IConfiguration _configuration;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        //构造函数注入
        public LMZTokenHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }
        /// <summary>
        /// 创建加密JwtToken
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="generateUser">登录成功的用户信息</param>
        /// <returns></returns>
        public string CreateJwtToken<T>(T generateUser)
        {
            try
            {
                // 1. 签名算法（比如HS256）
                var signingAlgorithm = SecurityAlgorithms.HmacSha256;

                // 2. 把用户信息转换成声明（Claim）列表，放到Token里
                var claimList = CreateClaimList(generateUser);

                // 3. 从配置里拿秘钥字符串，转成字节数组
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]);

                // 4. 把字节数组包装成 SymmetricSecurityKey，这个是JWT库用来代表密钥的类型
                var signingKey = new SymmetricSecurityKey(key);

                // 5. 创建签名凭证，告诉JWT“用这个密钥和这个算法签名”
                var signingCredentials = new SigningCredentials(signingKey, signingAlgorithm);


                var beijingTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "China Standard Time");
                // 6. 准备“身份信息”（Claims）准备“签名秘钥和算法”
                var token = new JwtSecurityToken(
                        issuer: _configuration["Jwt:Issuer"],//发布者
                        audience: _configuration["Jwt:Audience"],//接收者
                        claims: claimList,//存放的用户信息
                        notBefore: beijingTime,//发布时间
                        expires: beijingTime.AddMinutes(2),//有效期设置
                        signingCredentials: signingCredentials//数字签名
                    );

                // 6. 拼接JWT的各个部分
                string tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

                return tokenStr;
            }
            catch (Exception ex)
            {
                throw new Exception($"创建加密JwtToken{ex.Message}");
            }
        }
        /// <summary>
        /// 创建包含用户信息的Claim列表
        /// 把对象所有属性转换成一堆键值对（声明），写进 JWT 里，方便验证和识别用户信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="authUser"></param>
        /// <returns></returns>
        public List<Claim> CreateClaimList<T>(T authUser)
        {
            try
            {
                //显示转换举例
                //return new List<Claim>
                //    {
                //        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                //        new Claim(ClaimTypes.Name, user.UserName),
                //        new Claim("RealName", user.RealName ?? "")
                //    };

                // 获取类型信息
                var classType = typeof(T);
                //p.Name属性名称
                //p.GetValue(authUser)时获取属性值
                //p.GetValue(authUser)?.ToString() ?? string.Empty时获取属性值并转换为字符串，如果为null则返回空字符串
                return classType.GetProperties()
                    .Select(
                        p => new Claim(
                            p.Name, p.GetValue(authUser)?.ToString() ?? string.Empty)
                        ).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"创建包含用户信息的Claim列表{ex.Message}");
            }
        }
        /// <summary>
        /// 从 JWT Token 中提取出所有 Claim，并尝试转换成指定类型 T 的对象
        /// </summary>
        /// <typeparam name="T">目标类型，必须是一个无参构造类</typeparam>
        /// <param name="token">JWT 字符串</param>
        /// <returns>还原出来的 T 类型对象</returns>
        public T ConvertTokenToObj<T>(string token) where T : new()
        {
            var objA = new T();
            var jwtToken = _jwtSecurityTokenHandler.ReadJwtToken(token);

            foreach (var claim in jwtToken.Claims)
            {
                var property = typeof(T).GetProperties()
                    .FirstOrDefault(p => string.Equals(p.Name, claim.Type, StringComparison.OrdinalIgnoreCase));

                if (property == null || !property.CanWrite) continue;
                var targetType = property.PropertyType;

                var convertedvalue = Convert.ChangeType(claim.Value, targetType);
                property.SetValue(objA, convertedvalue);

            }

            return objA;
        }

        /// <summary>
        /// 生成一个安全的 Refresh Token（刷新令牌）
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public string GenerateRefreshToken(int size = 32)
        {
            // 1. 创建一个指定大小的字节数组
            var randomNumber = new byte[size];

            // 2. 使用加密安全的随机数生成器
            using (var rng = RandomNumberGenerator.Create())
            {
                // 3. 生成随机字节
                rng.GetBytes(randomNumber);

                // 4. 将随机字节转换为 Base64 字符串
                return Convert.ToBase64String(randomNumber);
            }
        }

        
    }
}
