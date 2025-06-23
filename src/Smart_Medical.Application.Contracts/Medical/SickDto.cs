using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Smart_Medical.Medical
{

    public class SickDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 病历状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 住院号
        /// </summary>
        public string InpatientNumber { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 出院科室
        /// </summary>
        public string DischargeDepartment { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 体温（℃）
        /// </summary>
        [Required]
        [Range(30, 45, ErrorMessage = "体温应在30~45℃之间")]
        public decimal Temperature { get; set; }

        /// <summary>
        /// 脉搏（次/min）
        /// </summary>
        [Required]
        [Range(20, 200, ErrorMessage = "脉搏应在20~200次/min之间")]
        public int Pulse { get; set; }

        /// <summary>
        /// 呼吸（次/min）
        /// </summary>
        [Required]
        [Range(5, 60, ErrorMessage = "呼吸应在5~60次/min之间")]
        public int Breath { get; set; }

        /// <summary>
        /// 血压（mmHg）
        /// </summary>
        [Required]
        [StringLength(16)]
        public string BloodPressure { get; set; }


        /// <summary>
        /// 出院时间
        /// </summary>
        public DateTime DischargeTime { get; set; }

        /// <summary>
        /// 入院诊断
        /// </summary>
        public string AdmissionDiagnosis { get; set; }

        /// <summary>
        /// 出院诊断
        /// </summary>
        public string DischargeDiagnosis { get; set; }
    }
}
