using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Smart_Medical.Pharmacy
{
    public class DrugDto : AuditedEntityDto<Guid>
    {
        public string DrugName { get; set; }
        public string DrugType { get; set; }
        public string FeeName { get; set; }
        public string DosageForm { get; set; }
        public string Specification { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public int Stock { get; set; }
        public int StockUpper { get; set; }
        public int StockLower { get; set; }
        public DateTime ProductionDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Effect { get; set; }
        public DrugCategory Category { get; set; }
    }
}
