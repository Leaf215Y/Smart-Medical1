using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Smart_Medical.Medical
{
    public interface IMedicalAppService : IApplicationService
    {
        Task<SickDto> GetSickAsync(Guid id);
        Task<PagedResultDto<SickDto>> GetLisSicktAsync(PagedAndSortedResultRequestDto input);
        Task<SickDto> CreateSickAsync(CreateUpdateSickDto input);
        Task<SickDto> UpdateSickAsync(Guid id, CreateUpdateSickDto input);
        Task DeleteSickAsync(Guid id);
    }
}
