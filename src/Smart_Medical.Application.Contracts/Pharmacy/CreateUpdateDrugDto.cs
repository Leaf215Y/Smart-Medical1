using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Medical.Pharmacy
{
    /// <summary>
    /// 药品创建/更新参数DTO
    /// </summary>
    public class CreateUpdateDrugDto
    {
        /// <summary>
        /// 药品名称（必填）
        /// </summary>
        [Required]
        [StringLength(128)]
        public string DrugName { get; set; }

        /// <summary>
        /// 药品类型（必填）
        /// </summary>
        [Required]
        [StringLength(32)]
        public string DrugType { get; set; }

        /// <summary>
        /// 费用名称（必填）
        /// </summary>
        [Required]
        [StringLength(32)]
        public string FeeName { get; set; }

        /// <summary>
        /// 剂型（必填）
        /// </summary>
        [Required]
        [StringLength(32)]
        public string DosageForm { get; set; }

        /// <summary>
        /// 规格（必填）
        /// </summary>
        [Required]
        [StringLength(64)]
        public string Specification { get; set; }

        /// <summary>
        /// 进价（元，必填）
        /// </summary>
        [Required]
        public decimal PurchasePrice { get; set; }

        /// <summary>
        /// 售价（元，必填）
        /// </summary>
        [Required]
        public decimal SalePrice { get; set; }

        /// <summary>
        /// 当前库存（必填）
        /// </summary>
        [Required]
        public int Stock { get; set; }

        /// <summary>
        /// 库存上限（必填）
        /// </summary>
        [Required]
        public int StockUpper { get; set; }

        /// <summary>
        /// 库存下限（必填）
        /// </summary>
        [Required]
        public int StockLower { get; set; }

        /// <summary>
        /// 生产日期（必填）
        /// </summary>
        [Required]
        public DateTime ProductionDate { get; set; }

        /// <summary>
        /// 有效期（必填）
        /// </summary>
        [Required]
        public DateTime ExpiryDate { get; set; }

        /// <summary>
        /// 药品功效（必填）
        /// </summary>
        [Required]
        [StringLength(256)]
        public string Effect { get; set; }

        /// <summary>
        /// 药品类别（必填，中药/西药等）
        /// </summary>
        [Required]
        public DrugCategory Category { get; set; }

        /// <summary>
        ///  供应商ID
        /// </summary>
        //public Guid? PharmaceuticalCompanyId { get; set; }
    }
}
