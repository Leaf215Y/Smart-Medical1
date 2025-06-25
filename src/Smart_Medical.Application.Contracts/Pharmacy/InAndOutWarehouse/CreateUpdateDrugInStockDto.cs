using System;
using System.ComponentModel.DataAnnotations;

namespace Smart_Medical.Pharmacy.InAndOutWarehouse
{
    /// <summary>
    /// 药品入库参数
    /// </summary>
    public class CreateUpdateDrugInStockDto
    {
        /// <summary>
        /// 药品ID
        /// </summary>
        [Required(ErrorMessage = "药品ID不能为空")]
        public Guid DrugId { get; set; }

        /// <summary>
        /// 入库数量
        /// 必须大于0
        /// </summary>
        [Required(ErrorMessage = "入库数量不能为空")]
        [Range(1, int.MaxValue, ErrorMessage = "入库数量必须大于0")]
        public int Quantity { get; set; }

        /// <summary>
        /// 入库单价（元）
        /// 必须大于0
        /// </summary>
        [Required(ErrorMessage = "入库单价不能为空")]
        [Range(0.01, double.MaxValue, ErrorMessage = "入库单价必须大于0")]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 生产日期
        /// 不能晚于当前时间
        /// </summary>
        [Required(ErrorMessage = "生产日期不能为空")]
        public DateTime ProductionDate { get; set; }

        /// <summary>
        /// 有效期
        /// 必须晚于生产日期
        /// </summary>
        [Required(ErrorMessage = "有效期不能为空")]
        public DateTime ExpiryDate { get; set; }

        /// <summary>
        /// 批号
        /// 用于追踪药品批次
        /// </summary>
        [Required(ErrorMessage = "批号不能为空")]
        [StringLength(32, ErrorMessage = "批号长度不能超过32个字符")]
        public string BatchNumber { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        [Required(ErrorMessage = "供应商不能为空")]
        [StringLength(100, ErrorMessage = "供应商名称长度不能超过100个字符")]
        public string Supplier { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(500, ErrorMessage = "备注长度不能超过500个字符")]
        public string Remarks { get; set; }
    }
} 