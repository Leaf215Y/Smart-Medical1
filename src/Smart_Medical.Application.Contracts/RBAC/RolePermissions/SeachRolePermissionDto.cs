using System;
using Volo.Abp.Application.Dtos;

namespace Smart_Medical.RBAC.RolePermissions
{
    public class SeachRolePermissionDto : PagedAndSortedResultRequestDto
    {
        public Guid? RoleId { get; set; }
        public Guid? PermissionId { get; set; }
    }
}