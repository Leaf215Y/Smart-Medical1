using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Smart_Medical.RBAC.Users
{
    public class LoginDto
    {
        [DefaultValue("admin")]
        [Required(ErrorMessage = "用户名是必填项。")]
        public string UserName { get; set; }

        [DefaultValue("123456")] 
        [Required(ErrorMessage = "密码是必填项。")]
        public string UserPwd { get; set; }


    }
}