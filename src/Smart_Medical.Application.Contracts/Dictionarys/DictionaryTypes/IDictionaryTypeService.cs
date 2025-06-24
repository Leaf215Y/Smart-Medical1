using Microsoft.AspNetCore.Mvc;
using Smart_Medical.Until;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Smart_Medical.Dictionarys.DictionaryTypes
{
    /// <summary>
    /// 字典类型IService
    /// </summary>
    public interface IDictionaryTypeService:IApplicationService
    {
        Task<ApiResult> InsertDictionaryDataLAsync(CreateUpdateDictionaryTypeDto input);
        Task<ApiResult> UpdateDictionaryDataLAsync(Guid id, CreateUpdateDictionaryTypeDto input);
        Task<ApiResult<PageResult<List<GetDictionaryTypeDto>>>> GetDictionaryDataList([FromQuery] GetDictionaryTypeSearchDto search);
        Task<ApiResult> DeleteDoctorDepartment(Guid id);

    }
}
