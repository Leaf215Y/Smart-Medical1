using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Smart_Medical.Pharmacy.InAndOutWarehouse
{
    /// <summary>
    /// 药品入库记录DTO
    /// </summary>
    public class DrugInStockDto 
    {
        /// <summary>
        /// 药品ID
        /// </summary>
        public int DrugId { get; set; }

        /// <summary>
        /// 药品名称
        /// </summary>
        public string DrugName { get; set; }

        /// <summary>
        /// 入库数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 入库单价
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 入库总金额
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime ProductionDate { get; set; }

        /// <summary>
        /// 有效期
        /// </summary>
        public DateTime ExpiryDate { get; set; }

        /// <summary>
        /// 批号
        /// </summary>
        public string BatchNumber { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public string Supplier { get; set; }

        /// <summary>
        /// 入库状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
    }
}
