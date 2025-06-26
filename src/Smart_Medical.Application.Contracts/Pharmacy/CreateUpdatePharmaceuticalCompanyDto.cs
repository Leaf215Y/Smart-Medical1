using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Medical.Pharmacy
{
    public class CreateUpdatePharmaceuticalCompanyDto
    {
        /// <summary>
        /// 公司id
        /// </summary>
        public Guid? CompanyId { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        [Required]
        [StringLength(128)]
        public string CompanyName { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [StringLength(64)]
        public string ContactPerson { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [StringLength(32)]
        public string ContactPhone { get; set; }

        /// <summary>
        /// 公司地址
        /// </summary>
        [StringLength(256)]
        public string Address { get; set; }
    }
}
