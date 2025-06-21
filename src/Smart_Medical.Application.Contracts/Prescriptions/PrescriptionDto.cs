using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Medical.Prescriptions
{
    public class PrescriptionDto
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
