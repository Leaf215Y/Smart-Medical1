using Volo.Abp.Application.Dtos;

namespace Smart_Medical.Application.Contracts.RBAC.Roles
{
    public class SeachRoleDto : PagedAndSortedResultRequestDto
    {
        public string RoleName { get; set; }
    }
}