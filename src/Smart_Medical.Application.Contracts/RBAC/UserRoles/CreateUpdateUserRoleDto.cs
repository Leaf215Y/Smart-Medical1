using System;
using System.ComponentModel.DataAnnotations;

namespace Smart_Medical.Application.Contracts.RBAC.UserRoles
{
    public class CreateUpdateUserRoleDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid RoleId { get; set; }
    }
} 