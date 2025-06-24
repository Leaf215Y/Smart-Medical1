using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Medical.Prescriptions
{
    public class MedicationSearchDto
    {
        public int PrescriptionId { get; set; }
        public int pageIndex { get; set; }

        public int pageSize { get; set; }

    }
}
