using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Medical.OutpatientClinic.Dtos.Parameter
{
    /// <summary>
    /// 处方
    /// </summary>
    public class PrescriptionDto
    {
        /// <summary>
        /// 患者编号
        /// </summary>
        public Guid PatientNumber { get; set; }

        /// <summary>
        /// 处方类型标记（1=西药，2=中药）
        /// </summary>
        public int PrescriptionTemplateNumber { get; set; }

        /// <summary>
        /// 药品项列表
        /// </summary>
        public List<PrescriptionItemDto> PrescriptionItems { get; set; } = new();
    }
    /// <summary>
    /// 药品项列表
    /// </summary>
    public class PrescriptionItemDto
    {
        /// <summary>
        /// 药品名称
        /// </summary>
        public string MedicationName { get; set; } = string.Empty;

        /// <summary>
        /// 规格
        /// </summary>
        public string Specification { get; set; } = string.Empty;

        /// <summary>
        /// 单价
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 每次剂量
        /// </summary>
        public int Dosage { get; set; }

        /// <summary>
        /// 剂量单位
        /// </summary>
        public string DosageUnit { get; set; } = string.Empty;

        /// <summary>
        /// 用法
        /// </summary>
        public string Usage { get; set; } = string.Empty;

        /// <summary>
        /// 频次
        /// </summary>
        public string Frequency { get; set; } = string.Empty;

        /// <summary>
        /// 药品总量
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 数量单位
        /// </summary>
        public string NumberUnit { get; set; } = string.Empty;

        /// <summary>
        /// 医嘱内容
        /// </summary>
        public string MedicalAdvice { get; set; } = string.Empty;
    }
}
