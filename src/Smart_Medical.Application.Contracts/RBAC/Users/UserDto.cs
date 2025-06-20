using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Medical.RBAC.Users
{
    /// <summary>
    /// 用户响应dto
    /// </summary>
    public class UserDto
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? UserPhone { get; set; }
        public bool UserSex { get; set; }
    }
    /// <summary>
    /// 登录dto
    /// </summary>
    public class LoginDto
    {
        [DefaultValue("admin")]
        [Required(ErrorMessage = "用户名是必填项。")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "用户名长度必须在 3 到 50 个字符之间。")]
        public string UserName { get; set; }

        [DefaultValue("123456")]
        [Required(ErrorMessage = "密码是必填项。")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "密码长度至少为 6 个字符。")]
        [DataType(DataType.Password)] // 提示此字段包含密码数据，用于 UI 渲染
        public string UserPwd { get; set; }
    }
}
