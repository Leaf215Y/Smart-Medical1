using Smart_Medical.Until;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Medical.Dictionarys.DictionaryTypes
{
    public class GetDictionaryTypeSearchDto:Seach
    {

        /// <summary>
        /// 字典标签/字典值
        /// </summary>
        public string? DictionaryLabel { get; set; }

    }
}
