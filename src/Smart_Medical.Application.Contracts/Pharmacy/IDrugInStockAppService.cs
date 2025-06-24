using Smart_Medical.Pharmacy.InAndOutWarehouse;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Smart_Medical.Pharmacy
{
    public interface IDrugInStockAppService : IApplicationService
    {
        Task StockInAsync(DrugInStockCreateDto input);
    }
}
