using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Smart_Medical.Pharmacy.InAndOutWarehouse
{
    public class DrugInStock : AuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 药品ID
        /// </summary>
        [Required]
        public Guid DrugId { get; set; }

        /// <summary>
        /// 医药公司ID
        /// </summary>
        [Required]
        public Guid PharmaceuticalCompanyId { get; set; }

        /// <summary>
        /// 入库数量
        /// </summary>
        [Required]
        public int Quantity { get; set; }

        /// <summary>
        /// 入库日期
        /// </summary>
        [Required]
        public DateTime StockInDate { get; set; }

        /// <summary>
        /// 批号
        /// </summary>
        [Required]
        [StringLength(64)]
        public string BatchNumber { get; set; }
    }
}
