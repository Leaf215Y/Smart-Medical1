using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Medical.Dictionarys.DictionaryTypes
{
    public class GetDictionaryTypeSearchDto
    {
        public string DictionaryTypeName { get; set; }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
