using System;
using System.ComponentModel.DataAnnotations;

namespace Smart_Medical.Application.Contracts.RBAC.RolePermissions
{
    public class CreateUpdateRolePermissionDto
    {
        [Required]
        public Guid RoleId { get; set; }

        [Required]
        public Guid PermissionId { get; set; }
    }
}