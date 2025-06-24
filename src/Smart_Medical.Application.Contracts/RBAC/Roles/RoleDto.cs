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

        public ICollection<UserRoleDto> UserRoles { get; set; }
        public ICollection<RolePermissionDto> RolePermissions { get; set; }
    }
}
