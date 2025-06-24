using System;
using System.ComponentModel.DataAnnotations;

namespace Smart_Medical.Pharmacy.InAndOutWarehouse
{
    public class DrugInStockCreateDto
    {
        /// <summary>
        /// ҩƷ��Ϣ id
        /// </summary>
        [Required]
        public int DrugId { get; set; }
        /// <summary>
        /// ҽҩ��˾ id
        /// </summary>
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