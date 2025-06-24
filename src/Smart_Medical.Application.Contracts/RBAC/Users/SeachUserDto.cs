using Volo.Abp.Application.Dtos;

namespace Smart_Medical.Application.Contracts.RBAC.Users
{
    public class SeachUserDto : PagedAndSortedResultRequestDto
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
    }
}