using Volo.Abp.Application.Dtos;

namespace Smart_Medical.Application.Contracts.RBAC.Permissions
{
    public class SeachPermissionDto : PagedAndSortedResultRequestDto
    {
        public string? PermissionName { get; set; } = string.Empty;
    }
}