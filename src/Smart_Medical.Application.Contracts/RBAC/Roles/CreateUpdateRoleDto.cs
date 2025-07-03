using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Smart_Medical.RBAC.Roles
{
    public class CreateUpdateRoleDto
    {
        [Required(ErrorMessage = "角色名称是必填项。")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "角色名称长度必须在 3 到 50 个字符之间。")]
        public string RoleName { get; set; }

        [Required(ErrorMessage = "角色描述是必填项。")]
        [StringLength(200, ErrorMessage = "角色描述不能超过200个字符！")]
        public string Description { get; set; }
    }
}
