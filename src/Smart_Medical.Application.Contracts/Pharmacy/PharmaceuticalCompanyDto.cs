using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Smart_Medical.Pharmacy
{
    public class PharmaceuticalCompanyDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPhone { get; set; }
        public string Address { get; set; }
    }
}
