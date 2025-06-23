using Smart_Medical.Until;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Smart_Medical.DispensingMedicineForAFee.IServices
{
    /// <summary>
    /// 收费发药
    /// </summary>
    public interface IDistri_MedicService : IApplicationService
    {
        Task<ApiResult> DistributeMedicine(Guid patientNumber);
    }
}
