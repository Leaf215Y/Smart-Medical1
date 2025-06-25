using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Smart_Medical.Application.Contracts.RBAC.UserRoles;
using Smart_Medical.Application.Contracts.RBAC.RolePermissions;

namespace Smart_Medical.Application.Contracts.RBAC.Roles
{
    public class RoleDto : AuditedEntityDto<Guid>
    {
        public string RoleName { get; set; }
        public string Description { get; set; }

        // 导航属性的DTO，用于展示关联的用户角色信息
        // 移除此属性以避免循环引用，因为 UserRoleDto 中已包含 RoleDto。
        // 如果需要获取角色的用户列表，请通过 UserRoleAppService 查询。
        // public ICollection<UserRoleDto> UserRoles { get; set; }

    
        public ICollection<UserRoleDto> UserRoles { get; set; }
        public ICollection<RolePermissionDto> RolePermissions { get; set; }
    }
}
