using Microsoft.AspNetCore.Mvc;
using Smart_Medical.Dictionarys.DictionaryDatas;
using Smart_Medical.Dictionarys.DictionaryTypes;
using Smart_Medical.Until;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Smart_Medical.Dictionarys
{
    /// <summary>
    /// 字典类型
    /// </summary>
    public class DictionaryTypeService: ApplicationService, IDictionaryTypeService
    {
        private readonly IRepository<DictionaryType, Guid> dictionarytype;

        public DictionaryTypeService(IRepository<DictionaryType, Guid> dictionarytype)
        {
            this.dictionarytype = dictionarytype;
        }
        /// <summary>
        /// 新增字典数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> InsertDictionaryDataLAsync(CreateUpdateDictionaryTypeDto input)
        {
            var res = ObjectMapper.Map<CreateUpdateDictionaryTypeDto, DictionaryType>(input);
            res = await dictionarytype.InsertAsync(res);
            return ApiResult.Success(ResultCode.Success);

        }
        /// <summary>
        /// 修改字典数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult> UpdateDictionaryDataLAsync(Guid id, CreateUpdateDictionaryTypeDto input)
        {
            var typelist = await dictionarytype.FindAsync(id);
            if (typelist.DictionaryValue == input.DictionaryValue)
            {
                return ApiResult.Fail("名称已存在已存在不能修改", ResultCode.NotFound);
            }
            var dto = ObjectMapper.Map(input, typelist);
            await dictionarytype.UpdateAsync(dto);
            return ApiResult.Success(ResultCode.Success);
        }

        /// <summary>
        /// 获取字典数据类型
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<PageResult<List<GetDictionaryTypeDto>>>> GetDictionaryDataList([FromQuery] GetDictionaryTypeSearchDto search)
        {
            var typelist = await dictionarytype.GetQueryableAsync();
            typelist= typelist.Where(x => x.DictionaryDataType.Contains(search.DictionaryTypeName));
            var res = typelist.PageResult(search.PageIndex, search.PageSize);
            var dto = ObjectMapper.Map<List<DictionaryType>, List<GetDictionaryTypeDto>>(res.Queryable.ToList());
            var pageinfo = new PageResult<List<GetDictionaryTypeDto>>
            {
                Data = dto,
                TotleCount = res.RowCount,
                TotlePage = (int)Math.Ceiling((double)res.RowCount / search.PageSize)
            };
            return ApiResult<PageResult<List<GetDictionaryTypeDto>>>.Success(pageinfo, ResultCode.Success);
        }

        /// <summary>
        /// 删除字典数据类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult> DeleteDoctorDepartment(Guid id)
        {
            var typelist = await dictionarytype.FindAsync(id);
            await dictionarytype.DeleteAsync(typelist);
            return ApiResult.Success(ResultCode.Success);
        }

    }
}
