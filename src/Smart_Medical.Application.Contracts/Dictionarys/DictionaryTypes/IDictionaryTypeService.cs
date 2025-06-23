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
    public interface IDictionaryTypeService:IApplicationService
    {
        Task<ApiResult<PageResult<List<GetDictionaryTypeDto>>>> GetDictionaryDataList([FromQuery] GetDictionaryTypeSearchDto search);
    }
}
