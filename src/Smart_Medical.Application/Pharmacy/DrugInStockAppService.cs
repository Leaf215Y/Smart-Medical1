using AutoMapper.Internal.Mappers;
using Smart_Medical.Pharmacy.InAndOutWarehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp;

namespace Smart_Medical.Pharmacy
{
    public class DrugInStockAppService : ApplicationService, IDrugInStockAppService
    {
        private readonly IRepository<DrugInStock, Guid> _drugInStockRepository;
        private readonly IRepository<Drug, Guid> _drugRepository;

        public DrugInStockAppService(
            IRepository<DrugInStock, Guid> drugInStockRepository,
            IRepository<Drug, Guid> drugRepository)
        {
            _drugInStockRepository = drugInStockRepository;
            _drugRepository = drugRepository;
        }

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
