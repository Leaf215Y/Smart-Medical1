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
        [Required]
        [StringLength(128)]
        public string CompanyName { get; set; }

        [StringLength(64)]
        public string ContactPerson { get; set; }

        [StringLength(32)]
        public string ContactPhone { get; set; }

        [StringLength(256)]
        public string Address { get; set; }
    }
}
