using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Medical.OutpatientClinic.Dtos.Parameter
{
    /// <summary>
    /// 医生为患者开具处方 DTO（一次完整开方）
    /// </summary>
    public class DoctorPrescriptionDto
    {
        /// <summary>
        /// 处方模板   编号0【无模板（非模板录入）】  
        /// Prescriptionid 处方信息id 关联 
        /// </summary>
        public int PrescriptionTemplateNumber { get; set; } = 0;

        /// <summary>
        /// 患者编号
        /// </summary>
        public Guid PatientNumber { get; set; }

        /// <summary>
        /// 是否使用处方模版
        /// </summary>
        public bool IsActive { get; set; } = true;
        /// <summary>
        /// 不使用处方模版时，  药品ID集合
        /// </summary>
        public string? DrugIds { get; set; } = string.Empty;

        /// <summary>
        /// 手动录入的药品明细（仅当 PrescriptionTemplateNumber 为 0 且 IsActive 为 false 时使用）
        /// </summary>
        public List<PrescriptionItemDto>? PrescriptionItems { get; set; }

        /// <summary>
        /// 医嘱内容  备注
        /// </summary>
        [StringLength(500, ErrorMessage = "医嘱内容长度不能超过500个字符")]
        public string? MedicalAdvice { get; set; } = string.Empty; 
    }

    /// <summary>
    /// 单个药品的处方明细 DTO（挂在 DoctorPrescriptionDto.PrescriptionItems 下）
    /// </summary>
    public class PrescriptionItemDto
    {
        /// <summary>
        /// 药品ID（关联 Drug.Id）
        /// </summary>
        public int DrugId { get; set; }

        /// <summary>
        /// 单次用药剂量（如 0.5、1.0 等）
        /// </summary>
        public decimal Dosage { get; set; }

        /// <summary>
        /// 剂量单位（如：片、ml、mg）
        /// </summary>
        public string DosageUnit { get; set; }

        /// <summary>
        /// 用法（如：口服、肌注、静滴等）
        /// </summary>
        public string Usage { get; set; }

        /// <summary>
        /// 用药频率（如：每日一次、每日三次、睡前服用等）
        /// </summary>
        public string Frequency { get; set; }

        /// <summary>
        /// 开药总数量（如开30片）
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 数量单位（如：片、瓶、支）
        /// </summary>
        public string NumberUnit { get; set; }

        /// <summary>
        /// 医嘱内容（医生额外说明，可选）
        /// </summary>
        public string? MedicalAdvice { get; set; }
    }
}
