using Smart_Medical.Pharmacy.InAndOutWarehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Smart_Medical.Pharmacy
{
    public interface IDrugInStockAppService : IApplicationService
    {
        Task<DrugDto> StockInAsync(CreateDrugInStockDto input);
    }
}
