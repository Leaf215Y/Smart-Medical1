using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Medical.OutpatientClinic.Dtos
{
    /// <summary>
    /// 患者所有病历信息
    /// </summary>
    public class GetSickInfoDto
    {
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
    }
}
