using AutoMapper.Internal.Mappers;
using Smart_Medical.Until;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Smart_Medical.Dictionarys
{

    public class DictionaryService: IDictionaryService, IApplicationService
    {
        private readonly IRepository<DictionaryData, Guid> dictionaryData;
        private readonly IRepository<DictionaryType, Guid> dictionarytype;

        public DictionaryService(IRepository<DictionaryData,Guid> dictionaryData, IRepository<DictionaryType,Guid> dictionarytype)
        {
            this.dictionaryData = dictionaryData;
            this.dictionarytype = dictionarytype;
        }
        /// <summary>
        /// 获取字典数据
        /// </summary>
        /// <param name="Dictiontype"></param>
        /// <returns></returns>
        public async Task<ApiResult<List<DictionaryDto>>> GetDictionaryDataListAsync(string Dictiontype)
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
