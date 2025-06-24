using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Smart_Medical.Pharmacy.InAndOutWarehouse
{
    public class DrugInStockDto : AuditedEntityDto<Guid>
    {
        public Guid DrugId { get; set; }
        public Guid PharmaceuticalCompanyId { get; set; }
        public int Quantity { get; set; }
        public DateTime StockInDate { get; set; }
        public string BatchNumber { get; set; }

        // Optional: Include names for display purposes
        public string DrugName { get; set; }
        public string PharmaceuticalCompanyName { get; set; }
    }
}
