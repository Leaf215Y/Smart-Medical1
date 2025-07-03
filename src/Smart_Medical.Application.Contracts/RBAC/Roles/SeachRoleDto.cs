using Volo.Abp.Application.Dtos;

namespace Smart_Medical.RBAC.Roles
{
    public class SeachRoleDto : PagedAndSortedResultRequestDto
    {
        public string? RoleName { get; set; } = string.Empty;
    }
}