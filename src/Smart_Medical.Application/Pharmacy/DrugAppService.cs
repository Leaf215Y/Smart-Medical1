using AutoMapper.Internal.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Smart_Medical.Pharmacy
{
    public class DrugAppService :
        CrudAppService<Drug, DrugDto, int, PagedAndSortedResultRequestDto, CreateUpdateDrugDto>,
        IDrugAppService
    {
        public DrugAppService(IRepository<Drug, int> repository)
            : base(repository)
        {

        }

        /// <summary>
        /// 新增药品
        /// Add a new drug
        /// </summary>
        /// <remarks>
        /// POST /api/app/pharmacy/drug
        /// 用于添加新的药品信息，需传入药品名称、类型、价格、库存等参数。
        /// </remarks>
        /// <param name="input">药品创建参数</param>
        /// <returns>操作结果，包含新增药品的详细信息</returns>
        public override async Task<DrugDto> CreateAsync(CreateUpdateDrugDto input)
        {


            // 名称唯一性
            var exists = await Repository.AnyAsync(d => d.DrugName == input.DrugName);
            if (exists)
                throw new UserFriendlyException($"药品名称 '{input.DrugName}' 已存在！");

            // 库存上下限
            if (input.StockLower > input.StockUpper)
                throw new UserFriendlyException("库存下限不能大于库存上限！");
            if (input.Stock < input.StockLower || input.Stock > input.StockUpper)
                throw new UserFriendlyException("库存必须在上下限之间！");

            // 价格校验
            if (input.PurchasePrice < 0 || input.SalePrice < 0)
                throw new UserFriendlyException("价格不能为负数！");
            if (input.PurchasePrice > input.SalePrice)
                throw new UserFriendlyException("进价不能大于售价！");

            // 日期校验
            if (input.ProductionDate > input.ExpiryDate)
                throw new UserFriendlyException("生产日期不能晚于有效期！");

            return await base.CreateAsync(input);
        }

        /// <summary>
        /// 删除药品
        /// Delete a drug
        /// </summary>
        /// <remarks>
        /// DELETE /api/app/pharmacy/drug/{id}
        /// 根据药品ID删除药品信息。
        /// </remarks>
        /// <param name="id">药品ID</param>
        /// <returns>无返回值，操作成功则表示删除成功</returns>
        public override async Task DeleteAsync(Guid id)
        {
            var drug = await Repository.FindAsync(id);
            if (drug == null)
            {
                throw new Volo.Abp.UserFriendlyException("药品不存在，无法删除！");
            }
            // 这里可以添加更多业务校验（如有关联数据禁止删除等）
            await base.DeleteAsync(id);
        }

        /// <summary>
        /// 更新药品信息
        /// Update drug information
        /// </summary>
        /// <remarks>
        /// PUT /api/app/pharmacy/drug/{id}
        /// 根据药品ID更新药品的详细信息。
        /// </remarks>
        /// <param name="id">药品ID</param>
        /// <param name="input">药品更新参数</param>
        /// <returns>操作结果，包含更新后的药品详细信息</returns>
        public override async Task<DrugDto> UpdateAsync(Guid id, CreateUpdateDrugDto input)
        {
            // 1. 校验药品是否存在
            var drug = await Repository.GetAsync(id);

            // 2. 校验名称唯一性（排除自己）
            var nameExists = await Repository.AnyAsync(d => d.DrugName == input.DrugName && d.Id != id);
            if (nameExists)
                throw new Volo.Abp.UserFriendlyException($"药品名称 '{input.DrugName}' 已存在！");

            // 3. 业务规则校验
            if (input.StockLower > input.StockUpper)
                throw new Volo.Abp.UserFriendlyException("库存下限不能大于库存上限！");
            if (input.Stock < input.StockLower || input.Stock > input.StockUpper)
                throw new Volo.Abp.UserFriendlyException("库存必须在上下限之间！");
            if (input.PurchasePrice < 0 || input.SalePrice < 0)
                throw new Volo.Abp.UserFriendlyException("价格不能为负数！");
            if (input.PurchasePrice > input.SalePrice)
                throw new Volo.Abp.UserFriendlyException("进价不能大于售价！");
            if (input.ProductionDate > input.ExpiryDate)
                throw new Volo.Abp.UserFriendlyException("生产日期不能晚于有效期！");

            // 4. 更新字段
            drug.DrugName = input.DrugName;
            drug.DrugType = input.DrugType;
            drug.FeeName = input.FeeName;
            drug.DosageForm = input.DosageForm;
            drug.Specification = input.Specification;
            drug.PurchasePrice = input.PurchasePrice;
            drug.SalePrice = input.SalePrice;
            drug.Stock = input.Stock;
            drug.StockUpper = input.StockUpper;
            drug.StockLower = input.StockLower;
            drug.ProductionDate = input.ProductionDate;
            drug.ExpiryDate = input.ExpiryDate;
            drug.Effect = input.Effect;
            drug.Category = input.Category;

            // 5. 保存
            await Repository.UpdateAsync(drug);

            // 6. 返回DTO
            return ObjectMapper.Map<Drug, DrugDto>(drug);
        }

        /// <summary>
        /// 获取药品分页列表
        /// Get paged drug list
        /// </summary>
        /// <remarks>
        /// GET /api/app/pharmacy/drugs
        /// 分页获取药品信息列表，可按名称、类型、类别等条件筛选。
        /// </remarks>
        /// <param name="name">药品名称（可选，模糊查询）</param>
        /// <param name="type">药品类型（可选）</param>
        /// <param name="category">药品类别（可选）</param>
        /// <param name="skipCount">跳过的记录数（分页）</param>
        /// <param name="maxResultCount">每页最大记录数</param>
        /// <returns>药品分页列表</returns>
        public async Task<PagedResultDto<DrugDto>> GetPagedListAsync(string name, string type, DrugCategory? category, int skipCount, int maxResultCount)
        {
            var query = await Repository.GetQueryableAsync();

            

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(d => d.DrugName.Contains(name));
            if (!string.IsNullOrWhiteSpace(type))
                query = query.Where(d => d.DrugType == type);
            if (category.HasValue)
                query = query.Where(d => d.Category == category.Value);

            var totalCount = await AsyncExecuter.CountAsync(query);
            var items = await AsyncExecuter.ToListAsync(
                query.Skip(skipCount).Take(maxResultCount)
            );
            return new PagedResultDto<DrugDto>(
                totalCount,
                ObjectMapper.Map<List<Drug>, List<DrugDto>>(items)
            );
        }
    }
}
