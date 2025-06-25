using AutoMapper.Internal.Mappers;
using Smart_Medical.Until;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;

namespace Smart_Medical.Pharmacy
{
    public class DrugAppService : ApplicationService, IDrugAppService
    {
        public IRepository<Drug, Guid> Repository { get; }

        public DrugAppService(IRepository<Drug, Guid> repository)
        {
            Repository = repository;
        }
        /// <summary>
        /// 根据Id获取药品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResult<DrugDto>> GetAsync(Guid id)
        {
            try
            {
                var drug = await Repository.FindAsync(id);
                if (drug == null)
                {
                    throw new Exception("药品不存在，无法删除！");
                }
                var result = ObjectMapper.Map<Drug, DrugDto>(drug);
                return ApiResult<DrugDto>.Success(result, ResultCode.Success);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 分页查询药品
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<PageResult<List<DrugDto>>>> GetListAsync([FromQuery] DrugSearchDto search)
        {
            var list = await Repository.GetQueryableAsync();

            // 按药品名称模糊查询
            if (!string.IsNullOrWhiteSpace(search.DrugName))
            {
                list = list.Where(x => x.DrugName.Contains(search.DrugName));
            }

            // 按药品类型模糊查询
            if (!string.IsNullOrWhiteSpace(search.DrugType))
            {
                list = list.Where(x => x.DrugType.Contains(search.DrugType));
            }

            // 按生产日期起筛选
            if (search.ProductionDateStart.HasValue)
            {
                list = list.Where(x => x.ProductionDate >= search.ProductionDateStart.Value);
            }

            // 按生产日期止筛选
            if (search.ProductionDateEnd.HasValue)
            {
                list = list.Where(x => x.ProductionDate <= search.ProductionDateEnd.Value);
            }

            // 按最小库存筛选
            if (search.StockMin.HasValue)
            {
                list = list.Where(x => x.Stock >= search.StockMin.Value);
            }

            // 按最大库存筛选
            if (search.StockMax.HasValue)
            {
                list = list.Where(x => x.Stock <= search.StockMax.Value);
            }

            // 分页处理
            var res = list.PageResult(search.pageIndex, search.pageSize);
            var dto = ObjectMapper.Map<List<Drug>, List<DrugDto>>(res.Queryable.ToList());
            var pageInfo = new PageResult<List<DrugDto>>
            {
                Data = dto,
                TotleCount = res.RowCount,
                TotlePage = (int)Math.Ceiling((double)res.RowCount / search.pageSize)
            };
            return ApiResult<PageResult<List<DrugDto>>>.Success(pageInfo, ResultCode.Success);
        }

        /// <summary>
        /// 添加药品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> CreateAsync(CreateUpdateDrugDto input)
        {
            var exists = await Repository.AnyAsync(d => d.DrugName == input.DrugName);
            if (exists)
                throw new UserFriendlyException($"药品名称 '{input.DrugName}' 已存在！");

            if (input.StockLower > input.StockUpper)
                throw new UserFriendlyException("库存下限不能大于库存上限！");
            if (input.Stock < input.StockLower || input.Stock > input.StockUpper)
                throw new UserFriendlyException("库存必须在上下限之间！");

            if (input.PurchasePrice < 0 || input.SalePrice < 0)
                throw new UserFriendlyException("价格不能为负数！");
            if (input.PurchasePrice > input.SalePrice)
                throw new UserFriendlyException("进价不能大于售价！");

            if (input.ProductionDate > input.ExpiryDate)
                throw new UserFriendlyException("生产日期不能晚于有效期！");

            var drug = ObjectMapper.Map<CreateUpdateDrugDto, Drug>(input);
            await Repository.InsertAsync(drug);

            return ApiResult.Success(ResultCode.Success);
        }
        /// <summary>
        /// 修改药品
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult> UpdateAsync(Guid id, CreateUpdateDrugDto input)
        {
            var drug = await Repository.GetAsync(id);

            var nameExists = await Repository.AnyAsync(d => d.DrugName == input.DrugName && d.Id != id);
            if (nameExists)
                throw new UserFriendlyException($"药品名称 '{input.DrugName}' 已存在！");

            if (input.StockLower > input.StockUpper)
                throw new UserFriendlyException("库存下限不能大于库存上限！");
            if (input.Stock < input.StockLower || input.Stock > input.StockUpper)
                throw new UserFriendlyException("库存必须在上下限之间！");
            if (input.PurchasePrice < 0 || input.SalePrice < 0)
                throw new UserFriendlyException("价格不能为负数！");
            if (input.PurchasePrice > input.SalePrice)
                throw new UserFriendlyException("进价不能大于售价！");
            if (input.ProductionDate > input.ExpiryDate)
                throw new UserFriendlyException("生产日期不能晚于有效期！");

            ObjectMapper.Map(input, drug);
            
            await Repository.UpdateAsync(drug);

            return ApiResult.Success(ResultCode.Success);
        }
        /// <summary>
        /// 删除药品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ApiResult> DeleteAsync(Guid id)
        {
            await Repository.DeleteAsync(id);
            return ApiResult.Success(ResultCode.Success);
        }
    }
}

