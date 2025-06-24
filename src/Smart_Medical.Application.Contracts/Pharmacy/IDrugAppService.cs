using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Smart_Medical.Pharmacy
{
    public interface IDrugAppService : IApplicationService
    {
        Task<DrugDto> GetAsync(int id);
        Task<PagedResultDto<DrugDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        Task<DrugDto> CreateAsync(CreateUpdateDrugDto input);
        Task<DrugDto> UpdateAsync(int id, CreateUpdateDrugDto input);
        Task DeleteAsync(int id);
    }
}
