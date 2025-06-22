using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Medical.OutpatientClinic.Dtos
{
    /// <summary>
    /// 就诊患者
    /// </summary>
    public class GetVisitingDto
    {
        /// <summary>
        /// 患者id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 患者姓名（必填）
        /// </summary>
        [Required(ErrorMessage = "患者姓名不能为空")]
        [StringLength(50, ErrorMessage = "患者姓名长度不能超过50个字符")]
        public string PatientName { get; set; } = string.Empty;

        /// <summary>
        /// 性别【1】男【2】女
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        [Range(0, 150, ErrorMessage = "年龄必须在0-150之间")]
        public int? Age { get; set; }

        /// <summary>
        /// 就诊日期
        /// </summary>
        public DateTime VisitDate { get; set; } = DateTime.Now;
    }
}
