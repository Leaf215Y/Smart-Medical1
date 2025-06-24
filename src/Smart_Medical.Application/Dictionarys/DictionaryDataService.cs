using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smart_Medical.Dictionarys.DictionaryDatas;
using Smart_Medical.Until;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Smart_Medical.Dictionarys
{
    /// <summary>
    /// 字典数据
    /// </summary>
    [ApiExplorerSettings(GroupName = "字典管理")]
    public class DictionaryDataService : ApplicationService, IDictionaryDataService
    {
        private readonly IRepository<DictionaryData, Guid> dictionaryData;
        private readonly IRepository<DictionaryType, Guid> dictionarytype;

        public DictionaryDataService(IRepository<DictionaryData, Guid> dictionaryData, IRepository<DictionaryType, Guid> dictionarytype)
        {
            this.dictionaryData = dictionaryData;
            this.dictionarytype = dictionarytype;
        }
        /// <summary>
        /// 新增字典数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> InsertDictionaryDataLAsync(CreateUpdateDictionaryDataDto input)
        {
            var res = ObjectMapper.Map<CreateUpdateDictionaryDataDto, DictionaryData>(input);
            res = await dictionaryData.InsertAsync(res);
            return ApiResult.Success(ResultCode.Success);

        }
        /// <summary>
        /// 修改字典数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult> UpdateDictionaryDataLAsync(Guid id, CreateUpdateDictionaryDataDto input)
        {
            var datalist = await dictionaryData.FindAsync(id);
            if (datalist.DictionaryDataName == input.DictionaryDataName)
            {
                return ApiResult.Fail("字典数据已存在不能修改", ResultCode.NotFound);
            }
            var dto = ObjectMapper.Map(input, datalist);
            await dictionaryData.UpdateAsync(dto);
            return ApiResult.Success(ResultCode.Success);
        }

        /// <summary>
        /// 获取字典数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<PageResult<List<GetDictionaryDataDto>>>> GetDictionaryDataList([FromQuery] GetDictionaryDataSearchDto search)
        {
            var datalist = await dictionaryData.GetQueryableAsync();
            var res = datalist.PageResult(search.PageIndex, search.PageSize);
            var dto = ObjectMapper.Map<List<DictionaryData>, List<GetDictionaryDataDto>>(res.Queryable.ToList());
            var pageinfo = new PageResult<List<GetDictionaryDataDto>>
            {
                Data = dto,
                TotleCount = res.RowCount,
                TotlePage = (int)Math.Ceiling((double)res.RowCount / search.PageSize)
            };
            return ApiResult<PageResult<List<GetDictionaryDataDto>>>.Success(pageinfo, ResultCode.Success);
        }
        /// <summary>
        /// 删除字典数据
        /// </summary>
        /// <param name="Dictiontype"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult> DeleteDictionaryListAsync(Guid id)
        {
            var datalist = await dictionaryData.FindAsync(id);
            await dictionaryData.DeleteAsync(datalist);
            return ApiResult.Success(ResultCode.Success);
        }
        /// <summary>
        /// 获取字典数据类型联查
        /// </summary>
        /// <param name="Dictiontype"></param>
        /// <returns></returns>
        public async Task<ApiResult<List<DictionaryDto>>> GetDictionaryListAsync(string Dictiontype)
        {
            var queryableData = await dictionaryData.GetQueryableAsync();
            var queryableType = await dictionarytype.GetQueryableAsync();

            var resultList = await (from a in queryableData
                                    join b in queryableType on a.DictionaryDataType equals b.DictionaryDataType
                                    // 优化2: 如果Dictiontype参数有值，添加过滤条件
                                    where !string.IsNullOrEmpty(Dictiontype) || a.DictionaryDataType == Dictiontype
                                    select new DictionaryDto
                                    {
                                        DictionaryDataState = a.DictionaryDataState,
                                        DictionaryDataName = a.DictionaryDataName,
                                        DictionaryDataType = a.DictionaryDataType,
                                        DictionarySort = b.DictionarySort,
                                        DictionaryLabel = b.DictionaryLabel,
                                        DictionaryValue = b.DictionaryValue,
                                        DictionaryTypeState = b.DictionaryTypeState
                                    }).ToListAsync(); // 优化3: 使用ToListAsync()异步执行查询

            return ApiResult<List<DictionaryDto>>.Success(resultList, ResultCode.Success);
        }
    }
}
