using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Smart_Medical.Prescriptions
{
    /// <summary>
    /// 用药
    /// </summary>
    public class Medication : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        ///  项目名称
        /// </summary>
        [Required]
        [StringLength(100)]
        public string MedicationName { get; set; }

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
        public string DosageUnit { get; set; }

        /// <summary>
        /// 用法
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Usage { get; set; }
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
        public string NumberUnit { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        [Required]
        [StringLength(50)]
        public decimal TotalPrice { get; set; }
        /// <summary>
        /// 处方Id
        /// </summary>
        [Required]
        public int PrescriptionId { get; set; }
    }
}
