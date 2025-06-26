using Volo.Abp.Application.Dtos;

namespace Smart_Medical.Application.Contracts.RBAC.Users
{
    public class SeachUserDto : PagedAndSortedResultRequestDto
    {
        public string? UserName { get; set; } = string.Empty;
        public string? UserEmail { get; set; } = string.Empty;
        public string? UserPhone { get; set; } = string.Empty;
    }
}