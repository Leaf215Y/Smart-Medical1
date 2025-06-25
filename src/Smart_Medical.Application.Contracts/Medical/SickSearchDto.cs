using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Smart_Medical.Medical
{
    public class SickSearchDto
    {
        /// <summary>
        /// 姓名 (可选, 模糊查询)
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 住院号 (可选, 模糊查询)
        /// </summary>
        public string? InpatientNumber { get; set; }

        /// <summary>
        /// 入院诊断 (可选, 模糊查询)
        /// </summary>
        public string? AdmissionDiagnosis { get; set; }

        /// <summary>
        /// 当前页码 (默认为1)
        /// </summary>
        [DefaultValue(1)]
        public int pageIndex { get; set; } = 1;

        /// <summary>
        /// 每页大小 (默认为10)
        /// </summary>
        [DefaultValue(10)]
        public int pageSize { get; set; } = 10;
    }
} 