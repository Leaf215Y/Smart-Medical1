using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Smart_Medical.DoctorvVsit
{
    /// <summary>
    /// 医生科室表
    /// </summary>
    public class DoctorDepartment : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 科室名称（必填）
        /// </summary>
        [Required(ErrorMessage = "科室名称不能为空")]
        [StringLength(50, ErrorMessage = "科室名称长度不能超过50个字符")]
        public string DepartmentName { get; set; } = string.Empty;

        /// <summary>
        /// 科室大类
        /// </summary>
        [StringLength(30, ErrorMessage = "科室大类长度不能超过30个字符")]
        public string DepartmentCategory { get; set; } = string.Empty;

        /// <summary>
        /// 科室地址ss
        /// </summary>
        [StringLength(100, ErrorMessage = "科室地址长度不能超过100个字符")]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// 科室负责人姓名
        /// </summary>
        [StringLength(20, ErrorMessage = "负责人姓名长度不能超过20个字符")]
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
        [StringLength(20, ErrorMessage = "科室类型长度不能超过20个字符")]
        public string? Type { get; set; }
    }
}
