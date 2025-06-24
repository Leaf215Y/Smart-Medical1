using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Medical.Pharmacy.InAndOutWarehouse
{
    public class CreateDrugInStockDto
    {
        [Required]
        public Guid DrugId { get; set; }

        [Required]
        public Guid PharmaceuticalCompanyId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        public DateTime StockInDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(64)]
        public string BatchNumber { get; set; }
    }
}
