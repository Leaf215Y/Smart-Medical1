using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Smart_Medical.Application.Contracts.RBAC.Permissions
{
    public class CreateUpdatePermissionDto
    {
        [Required(ErrorMessage = "权限名称是必填项。")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "权限名称长度必须在 3 到 50 个字符之间。")]
        public string PermissionName { get; set; }
    }
}