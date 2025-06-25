using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Smart_Medical.Application.Contracts.RBAC.UserRoles; // 引用Contracts层的UserRoleDto

namespace Smart_Medical.Application.Contracts.RBAC.Users
{
    /// <summary>
    /// 用户响应dto
    /// </summary>
    public class UserDto : AuditedEntityDto<Guid>
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public bool? UserSex { get; set; }


        // 导航属性的DTO，用于展示关联的用户角色信息
        // 移除此属性以避免循环引用，因为 UserRoleDto 中已包含 UserDto。
        // 如果需要获取用户的角色列表，请通过 UserRoleAppService 查询。
        // public ICollection<UserRoleDto> UserRoles { get; set; }
    }
}
