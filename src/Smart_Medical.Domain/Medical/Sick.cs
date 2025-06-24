using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Smart_Medical.Medical
{
    public class Sick : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 患者基本信息Id
        /// </summary>
        [Required(ErrorMessage = "患者基本信息Id不能为空")]
        public Guid BasicPatientId { get; set; }

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
        public string? InpatientNumber { get; set; } = string.Empty;



        /// <summary>
        /// 出院时间
        /// </summary>
        [Required]
        public DateTime? DischargeTime { get; set; } = null;

        /// <summary>
        /// 入院诊断
        /// </summary>
        [Required]
        [StringLength(128)]
        public string? AdmissionDiagnosis { get; set; } = string.Empty;


        /// <summary>
        /// 体温（℃）
        /// </summary>
        [Required]
        [Range(30, 45, ErrorMessage = "体温应在30~45℃之间")]
        public decimal? Temperature { get; set; } =0;

        /// <summary>
        /// 脉搏（次/min）
        /// </summary>
        [Required]
        [Range(20, 200, ErrorMessage = "脉搏应在20~200次/min之间")]
        public int? Pulse { get; set; } = 0;

        /// <summary>
        /// 呼吸（次/min）
        /// </summary>
        [Required]
        [Range(5, 60, ErrorMessage = "呼吸应在5~60次/min之间")]
        public int? Breath { get; set; } = 0;

        /// <summary>
        /// 血压（mmHg）
        /// </summary>
        [Required]
        [StringLength(16)]
        public string? BloodPressure { get; set; }=string.Empty;

       
    }
}
