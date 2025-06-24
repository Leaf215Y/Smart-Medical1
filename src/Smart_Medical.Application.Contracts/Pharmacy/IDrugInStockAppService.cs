using Smart_Medical.Pharmacy.InAndOutWarehouse;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Smart_Medical.Pharmacy
{
    /// <summary>
    /// 药品入库管理服务接口
    /// </summary>
    public interface IDrugInStockAppService : IApplicationService
    {
        Task StockInAsync(DrugInStockCreateDto input);
    }
}
