using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Smart_Medical.Prescriptions
{
    /// <summary>
    /// 处方信息
    /// </summary>
    public class PrescriptionAs : FullAuditedAggregateRoot<int>
    {

        /// <summary>
        /// 处方名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public string PrescriptionName { get; set; }
        /// <summary>
        /// 父级Id
        /// </summary>
        [Required]
        public int ParentId { get; set; } 


    }
}
