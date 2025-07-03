using System;
using Volo.Abp.Application.Dtos;

namespace Smart_Medical.RBAC.UserRoles
{
    public class SeachUserRoleDto : PagedAndSortedResultRequestDto
    {
        public Guid? UserId { get; set; }
        public Guid? RoleId { get; set; }
    }
}