using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Smart_Medical.Patient
{
    /// <summary>
    /// 患者开具处方
    /// </summary>
    public class PatientPrescription : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 处方模板编号【可为0，为0则不属于任何模板】
        /// </summary>
        public int PrescriptionTemplateNumber { get; set; } = 0;

        /// <summary>
        /// 患者编号
        /// </summary>
        public Guid PatientNumber { get; set; }

        #region   药品信息

        /// <summary>
        ///  药品名称
        /// </summary>
        [Required]
        [StringLength(100)]
        public string MedicationName { get; set; } = string.Empty;

        /// <summary>
        /// 规格
        /// </summary>
        [StringLength(100)]
        public string Specification { get; set; } = string.Empty;
        /// <summary>
        /// 单价
        /// </summary>
        [Required]
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// 每次剂量
        /// </summary>
        [Required]
        public int Dosage { get; set; }
        /// <summary>
        /// 剂量单位
        /// </summary>
        [Required]
        [StringLength(20)]
        public string DosageUnit { get; set; } = string.Empty;

        /// <summary>
        /// 用法
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Usage { get; set; } = string.Empty;
        /// <summary>
        /// 频次
        /// </summary>
        [StringLength(20)]
        public string Frequency { get; set; } = string.Empty;
        /// <summary>
        /// 药品总量
        /// </summary>
        [Required]
        public int Number { get; set; }
        /// <summary>
        /// 数量单位
        /// </summary>
        [Required]
        [StringLength(20)]
        public string NumberUnit { get; set; } = string.Empty;

        /// <summary>
        /// 总金额
        /// </summary>
        public decimal TotalPrice { get; set; }
        /// <summary>
        /// 处方Id
        /// </summary>
        [Required]
        public int PrescriptionId { get; set; }

        #endregion        

        /// <summary>
        /// 医嘱内容
        /// </summary>
        [StringLength(200, ErrorMessage = "医嘱内容长度不能超过200个字符")]
        public string MedicalAdvice { get; set; } = string.Empty;
    }
}
