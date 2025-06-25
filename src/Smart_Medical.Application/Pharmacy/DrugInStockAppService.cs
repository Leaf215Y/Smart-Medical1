using Microsoft.AspNetCore.Mvc;
using Smart_Medical.Pharmacy.InAndOutWarehouse;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp;
using Smart_Medical.Until;
using Microsoft.AspNetCore.Mvc;

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
        [HttpPost]
        public async Task<ApiResult<DrugInStockDto>> StockInAsync(CreateUpdateDrugInStockDto input)
        {
            // 1. 查找药品实体
            var drug = await _drugRepository.FindAsync(input.DrugId);
            if (drug == null)
            {
                return ApiResult<DrugInStockDto>.Fail("找不到对应的药品，请检查药品ID是否正确。", ResultCode.NotFound);
            }

            // 2. 校验生产日期、有效期
            if (input.ProductionDate > DateTime.Now)
            {
                return ApiResult<DrugInStockDto>.Fail("生产日期不能晚于当前时间", ResultCode.ValidationError);
            }
            if (input.ExpiryDate <= input.ProductionDate)
            {
                return ApiResult<DrugInStockDto>.Fail("有效期必须晚于生产日期", ResultCode.ValidationError);
            }

            // 3. 校验库存上下限
            int newStock = drug.Stock + input.Quantity;
            if (newStock > drug.StockUpper)
            {
                return ApiResult<DrugInStockDto>.Fail($"入库后库存({newStock})超过上限({drug.StockUpper})", ResultCode.ValidationError);
            }
            if (newStock < drug.StockLower)
            {
                return ApiResult<DrugInStockDto>.Fail($"入库后库存({newStock})低于下限({drug.StockLower})", ResultCode.ValidationError);
            }

            // 4. 更新药品库存
            drug.Stock = newStock;
            await _drugRepository.UpdateAsync(drug);

            // 5. 创建入库记录
            var drugInStock = new DrugInStock
            {
                DrugId = input.DrugId,
                Quantity = input.Quantity,
                UnitPrice = input.UnitPrice,
                TotalAmount = input.UnitPrice * input.Quantity,
                ProductionDate = input.ProductionDate,
                ExpiryDate = input.ExpiryDate,
                BatchNumber = input.BatchNumber,
                Supplier = input.Supplier,
                Status = "已入库",
                CreationTime = DateTime.Now
            };
            await _drugInStockRepository.InsertAsync(drugInStock);

            // 6. 映射为DrugInStockDto
            var dto = new DrugInStockDto
            {
                DrugId = drugInStock.DrugId,
                Quantity = drugInStock.Quantity,
                UnitPrice = drugInStock.UnitPrice,
                TotalAmount = drugInStock.TotalAmount,
                ProductionDate = drugInStock.ProductionDate,
                ExpiryDate = drugInStock.ExpiryDate,
                BatchNumber = drugInStock.BatchNumber,
                Supplier = drugInStock.Supplier,
                Status = drugInStock.Status,
                // CreationTime 字段在Dto基类AuditedEntityDto<Guid>中
                Remarks = input.Remarks
            };

            return ApiResult<DrugInStockDto>.Success(dto, ResultCode.Success);
        }

    }
}
