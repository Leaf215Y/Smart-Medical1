using Smart_Medical.Until;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Medical.Dictionarys
{
    public interface IDictionaryService
    {
        /// <summary>
        /// 获取字典数据
        /// </summary>
        /// <param name="Dictiontype"></param>
        /// <returns></returns>
        Task<ApiResult<List<DictionaryDto>>> GetDictionaryDataListAsync(string Dictiontype);
    }
}
