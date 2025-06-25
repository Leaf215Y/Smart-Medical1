using Microsoft.AspNetCore.Mvc;
using Smart_Medical.Pharmacy.InAndOutWarehouse;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Smart_Medical.Pharmacy
{
    [ApiExplorerSettings(GroupName = "药品入库管理")]
    public class DrugInStockAppService : ApplicationService, IDrugInStockAppService
    {
        private readonly IRepository<DrugInStock, Guid> _drugInStockRepository;
        private readonly IRepository<Drug, int> _drugRepository;

        public DrugInStockAppService(
            IRepository<DrugInStock, Guid> drugInStockRepository,
            IRepository<Drug, int> drugRepository)
        {
            _drugInStockRepository = drugInStockRepository;
            _drugRepository = drugRepository;
        }

        /// <summary>
        /// 药品入库
        /// Drug stock-in (add inventory record)
        /// </summary>
        /// <remarks>
        /// POST /api/app/pharmacy/drug-in-stock
        /// 用于将药品入库，增加库存并记录入库明细。
        /// </remarks>
        /// <param name="input">药品入库参数，包括药品ID、供应商ID、数量、入库日期、批号等</param>
        /// <returns>无返回值，操作成功即表示入库成功</returns>
        public async Task StockInAsync(DrugInStockCreateDto input)
        {
            // 1. 查找药品实体，如果不存在，GetAsync会自动抛出EntityNotFoundException
            var drug = await _drugRepository.GetAsync(input.DrugId);

            // 2. 更新药品的总库存
            drug.Stock += input.Quantity;
            await _drugRepository.UpdateAsync(drug);

            // 3. 创建入库记录
            var drugInStock = new DrugInStock
            {
                //Id = input.DrugId,
                //PharmaceuticalCompanyId = input.PharmaceuticalCompanyId,
                Quantity = input.Quantity,
                StockInDate = input.StockInDate,
                BatchNumber = input.BatchNumber
            };
            await _drugInStockRepository.InsertAsync(drugInStock);

            // (可选业务逻辑) 可以在这里检查库存是否超过上限或低于下限，并触发相应事件
            // if (drug.Stock < drug.StockLower) { ... }
            // if (drug.Stock > drug.StockUpper) { ... }

        }

    }
}
