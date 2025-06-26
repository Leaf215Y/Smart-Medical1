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
        [Required(ErrorMessage = "药品名称不能为空")]
        [StringLength(100, ErrorMessage = "药品名称长度不能超过100个字符")]
        public string DrugName { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "药品类型长度不能超过50个字符")]
        public string DrugType { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "费用名称长度不能超过50个字符")]
        public string FeeName { get; set; } = string.Empty;

        [StringLength(30, ErrorMessage = "剂型长度不能超过30个字符")]
        public string DosageForm { get; set; } = string.Empty;

        [StringLength(64, ErrorMessage = "规格长度不能超过64个字符")]
        public string Specification { get; set; } = string.Empty;

        [Required(ErrorMessage = "进价不能为空")]
        public decimal PurchasePrice { get; set; }

        [Required(ErrorMessage = "售价不能为空")]
        public decimal SalePrice { get; set; }

        [Required(ErrorMessage = "库存不能为空")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "库存上限不能为空")]
        public int StockUpper { get; set; }

        [Required(ErrorMessage = "库存下限不能为空")]
        public int StockLower { get; set; }

        [Required(ErrorMessage = "生产日期不能为空")]
        public DateTime ProductionDate { get; set; }

        [Required(ErrorMessage = "有效期不能为空")]
        public DateTime ExpiryDate { get; set; }

        [StringLength(255, ErrorMessage = "药效长度不能超过255个字符")]
        public string Effect { get; set; } = string.Empty;

        [Required(ErrorMessage = "药品类别不能为空")]
        public DrugCategory Category { get; set; }

        public Guid? PharmaceuticalCompanyId { get; set; } = null;
    }
}

