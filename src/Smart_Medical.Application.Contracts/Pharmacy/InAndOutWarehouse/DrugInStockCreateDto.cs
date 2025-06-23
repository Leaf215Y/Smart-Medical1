using System;
using System.ComponentModel.DataAnnotations;

namespace Smart_Medical.Pharmacy.InAndOutWarehouse
{
    public class DrugInStockCreateDto
    {
        [Required]
        public Guid DrugId { get; set; }

        [Required]
        public Guid PharmaceuticalCompanyId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        public DateTime StockInDate { get; set; }

        [Required]
        [StringLength(64)]
        public string BatchNumber { get; set; }
    }
} 