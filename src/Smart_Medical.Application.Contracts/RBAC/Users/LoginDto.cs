using System.ComponentModel.DataAnnotations;

namespace Smart_Medical.Application.Contracts.RBAC.Users
{
    public class LoginDto
    {
        [Required(ErrorMessage = "用户名是必填项。")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "密码是必填项。")]
        public string UserPwd { get; set; }
    }
}