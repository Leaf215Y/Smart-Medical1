using System;
using Volo.Abp.Application.Dtos;
using Smart_Medical.Application.Contracts.RBAC.Roles; // 引用Contracts层的RoleDto
using Smart_Medical.Application.Contracts.RBAC.Permissions; // 引用Contracts层的PermissionDto

namespace Smart_Medical.Application.Contracts.RBAC.RolePermissions
{
    public class RolePermissionDto : AuditedEntityDto<Guid>
    {
        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }

        // 导航属性的DTO，用于展示关联的角色和权限信息
        public RoleDto Role { get; set; }
        public PermissionDto Permission { get; set; }
    }
}