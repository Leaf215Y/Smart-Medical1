using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Medical.Pharmacy
{
    public class CreateUpdateDrugDto
    {
        [Required]
        [StringLength(128)]
        public string DrugName { get; set; }

        [Required]
        [StringLength(32)]
        public string DrugType { get; set; }

        [Required]
        [StringLength(32)]
        public string FeeName { get; set; }

        [Required]
        [StringLength(32)]
        public string DosageForm { get; set; }

        [Required]
        [StringLength(64)]
        public string Specification { get; set; }

        [Required]
        public decimal PurchasePrice { get; set; }

        [Required]
        public decimal SalePrice { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        public int StockUpper { get; set; }

        [Required]
        public int StockLower { get; set; }

        [Required]
        public DateTime ProductionDate { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        [StringLength(256)]
        public string Effect { get; set; }

        [Required]
        public DrugCategory Category { get; set; }
    }
}
