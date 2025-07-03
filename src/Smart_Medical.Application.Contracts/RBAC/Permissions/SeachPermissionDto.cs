using Volo.Abp.Application.Dtos;

namespace Smart_Medical.RBAC.Permissions
{
    public class SeachPermissionDto : PagedAndSortedResultRequestDto
    {
        public string? PermissionName { get; set; } = string.Empty;
    }
}