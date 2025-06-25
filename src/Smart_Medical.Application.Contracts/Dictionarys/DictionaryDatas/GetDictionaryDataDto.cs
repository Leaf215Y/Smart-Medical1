using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Medical.Dictionarys.DictionaryDatas
{
    /// <summary>
    /// 字典数据dto
    /// </summary>
    public class GetDictionaryDataDto
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 字典数据名称
        /// </summary>
        public string DictionaryDataName { get; set; }
        /// <summary>
        /// 字典数据类型
        /// </summary>
        public string DictionaryDataType { get; set; }
        /// <summary>
        /// 字典数据状态
        /// </summary>
        public int DictionaryDataState { get; set; }
        /// <summary>
        /// 字典数据描述
        /// </summary>
        public string DictionaryDataDesc { get; set; }
    }
}
