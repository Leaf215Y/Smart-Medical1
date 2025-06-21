using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Medical.Medical
{
    public class CreateUpdateSickDto
    {
        /// <summary>
        /// 病历状态
        /// </summary>
        [Required]
        [StringLength(32)]
        public string Status { get; set; }

        /// <summary>
        /// 住院号
        /// </summary>
        [Required]
        [StringLength(32)]
        public string InpatientNumber { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [StringLength(32)]
        public string Name { get; set; }

        /// <summary>
        /// 出院科室
        /// </summary>
        [Required]
        [StringLength(64)]
        public string DischargeDepartment { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Required]
        [StringLength(8)]
        public string Gender { get; set; }

        /// <summary>
        /// 出院时间
        /// </summary>
        [Required]
        public DateTime DischargeTime { get; set; }

        /// <summary>
        /// 入院诊断
        /// </summary>
        [Required]
        [StringLength(128)]
        public string AdmissionDiagnosis { get; set; }

        /// <summary>
        /// 出院诊断
        /// </summary>
        [Required]
        [StringLength(128)]
        public string DischargeDiagnosis { get; set; }
    }
}
