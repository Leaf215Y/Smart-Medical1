using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Medical.DoctorvVsit.DockerDepartments
{
    /// <summary>
    /// 科室Dto显示
    /// </summary>
    public class GetDoctorDepartmentListDto
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 科室名称（必填）
        /// </summary>
        public string DepartmentName { get; set; } = string.Empty;

        /// <summary>
        /// 科室大类
        /// </summary>
        public string DepartmentCategory { get; set; } = string.Empty;

        /// <summary>
        /// 科室地址
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// 科室负责人姓名
        /// </summary>
        public string DirectorName { get; set; } = string.Empty;

        /// <summary>
        /// 医师人数
        /// </summary>
        public int DoctorCount { get; set; }

        /// <summary>
        /// 药师人数
        /// </summary>
        public int PharmacistCount { get; set; }

        /// <summary>
        /// 护士人数
        /// </summary>
        public int NurseCount { get; set; }

        /// <summary>
        /// 科室类型
        /// </summary>
        public string? Type { get; set; }
    }
}
