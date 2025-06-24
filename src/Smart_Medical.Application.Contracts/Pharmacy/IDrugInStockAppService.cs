using Microsoft.AspNetCore.Mvc;
using Smart_Medical.Pharmacy.InAndOutWarehouse;
using Smart_Medical.Until;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Smart_Medical.Pharmacy
{
    /// <summary>
    /// 药品入库服务接口
    /// </summary>
    public interface IDrugInStockAppService : IApplicationService
    {
        /// <summary>
        /// 药品入库
        /// </summary>
        /// <param name="input">入库参数</param>
        /// <returns></returns>
        Task<ApiResult<DrugInStockDto>> StockInAsync(CreateUpdateDrugInStockDto input);
    }
}
