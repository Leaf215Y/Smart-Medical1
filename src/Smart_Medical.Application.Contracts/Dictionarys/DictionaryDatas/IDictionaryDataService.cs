﻿using Microsoft.AspNetCore.Mvc;
using Smart_Medical.Dictionarys.DictionaryTypes;
using Smart_Medical.Until;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Medical.Dictionarys.DictionaryDatas
{
    public interface IDictionaryDataService
    {

        Task<ApiResult<PageResult<List<GetDictionaryDataDto>>>> GetDictionaryDataList([FromQuery] GetDictionaryDataSearchDto search);
        Task<ApiResult<List<DictionaryDto>>> GetDictionaryListAsync(string Dictiontype);

    }
}
