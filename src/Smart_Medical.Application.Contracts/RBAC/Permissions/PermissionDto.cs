using Smart_Medical.Enums;
using Smart_Medical.RBAC;
using System;
using Volo.Abp.Application.Dtos;

namespace Smart_Medical.Application.Contracts.RBAC.Permissions
{
    public class PermissionDto : AuditedEntityDto<Guid>
    {
        public string PermissionName { get; set; }
        public string PermissionCode { get; set; }
        public PermissionType Type { get; set; }
        public string PagePath { get; set; }
        public Guid? ParentId { get; set; }
    }

    }
}