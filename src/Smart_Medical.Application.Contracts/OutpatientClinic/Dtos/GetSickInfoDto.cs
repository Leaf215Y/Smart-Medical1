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
        /// 体温（℃）【BasicPatientInfo】
        /// </summary>
        [Required]
        [Range(30, 45, ErrorMessage = "体温应在30~45℃之间")]
        public decimal Temperature { get; set; }

        /// <summary>
        /// 脉搏（次/min）【BasicPatientInfo】
        /// </summary>
        [Required]
        [Range(20, 200, ErrorMessage = "脉搏应在20~200次/min之间")]
        public int Pulse { get; set; }

        /// <summary>
        /// 呼吸（次/min）【BasicPatientInfo】
        /// </summary>
        [Required]
        [Range(5, 60, ErrorMessage = "呼吸应在5~60次/min之间")]
        public int Breath { get; set; }

        /// <summary>
        /// 血压（mmHg）【BasicPatientInfo】
        /// </summary>
        [Required]
        [StringLength(16)]
        public string BloodPressure { get; set; }

        /// <summary>
        /// 主诉 【DoctorClinic】
        /// 患者本次就诊的主要症状或不适的简要描述。
        /// </summary>
        [StringLength(500, ErrorMessage = "主诉内容不能超过500个字符！")]
        public string? ChiefComplaint { get; set; }

        /// <summary>
        /// 不使用处方模版时，  药品ID集合【PatientPrescription】
        /// </summary>
        public string? DrugIds { get; set; } = string.Empty;

        /// <summary>
        /// 处方模板编号0【无模板（非模板录入）】1【西药处方】2【中药处方】【PatientPrescription】
        /// </summary>
        public int PrescriptionTemplateNumber { get; set; } = 0;

        /// <summary>
        /// 医嘱内容【PatientPrescription】
        /// </summary>
        [StringLength(200, ErrorMessage = "医嘱内容长度不能超过200个字符")]
        public string MedicalAdvice { get; set; } = string.Empty;
    }
}
