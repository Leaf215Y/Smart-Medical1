using System;
using Volo.Abp.Application.Dtos;

namespace Smart_Medical.Application.Contracts.RBAC.Permissions
{
    public class PermissionDto : AuditedEntityDto<Guid>
    {
        public string PermissionName { get; set; }
    }
}