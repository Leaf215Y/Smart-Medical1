using AutoMapper.Internal.Mappers;
using Smart_Medical.Pharmacy.InAndOutWarehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

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

        public async Task<DrugDto> StockInAsync(CreateDrugInStockDto input)
        {
            // 1. 创建入库记录
            // 将输入的DTO映射为DrugInStock实体
            var drugInStock = ObjectMapper.Map<CreateDrugInStockDto, DrugInStock>(input);
            // 将入库记录插入数据库
            await _drugInStockRepository.InsertAsync(drugInStock);

            // 2. 更新药品的总库存信息
            // 根据DrugId获取药品实体
            var drug = await _drugRepository.GetAsync(input.DrugId);
            // 增加药品的库存数量
            drug.Stock += input.Quantity;
            // (可选) 更新药品的默认供应商为本次入库的供应商
            drug.PharmaceuticalCompanyId = input.PharmaceuticalCompanyId;
            // 更新药品信息到数据库
            await _drugRepository.UpdateAsync(drug);

            // 3. 将更新后的药品实体映射为DTO并返回
            return ObjectMapper.Map<Drug, DrugDto>(drug);
        }
    }
}
