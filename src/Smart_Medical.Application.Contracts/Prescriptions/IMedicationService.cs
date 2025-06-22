using Microsoft.AspNetCore.Mvc;
using Smart_Medical.Until;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Smart_Medical.Prescriptions
{
    public interface IMedicationService : IApplicationService
    {
        Task<ApiResult> CreateAsync(CreateUpdateMedicationDto input);
        Task<ApiResult<PageResult<List<MedicationDto>>>> GetMedicationList([FromQuery] MedicationSearchDto search);
    }
}
