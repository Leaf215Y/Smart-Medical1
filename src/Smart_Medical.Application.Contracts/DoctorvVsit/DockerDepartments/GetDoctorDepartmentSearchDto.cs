using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Medical.DoctorvVsit.DockerDepartments
{
    public class GetDoctorDepartmentSearchDto
    {
        public string? DepartmentName { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

    }
}
