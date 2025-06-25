using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Medical.OutpatientClinic.Dtos
{
    /// <summary>
    /// 快速就诊、登记患者信息 DTO（合并多个实体）
    /// </summary>
    public class InsertPatientDto
    {
        // ===================== 来自：BasicPatientInfo =====================

        /// <summary>患者姓名（必填）</summary>
        [Required(ErrorMessage = "患者姓名不能为空")]
        [StringLength(50, ErrorMessage = "患者姓名长度不能超过50个字符")]
        public string PatientName { get; set; } = string.Empty;

        /// <summary>性别【1】男【2】女</summary>
        public int Gender { get; set; }

        /// <summary>年龄</summary>
        [Range(0, 150, ErrorMessage = "年龄必须在0-150之间")]
        public int? Age { get; set; }

        /// <summary>年龄单位（年/月/日）</summary>
        [StringLength(10, ErrorMessage = "年龄单位长度不能超过10个字符")]
        public string AgeUnit { get; set; } = string.Empty;

        /// <summary>联系方式</summary>
        [StringLength(20, ErrorMessage = "联系方式长度不能超过20个字符")]
        [Phone(ErrorMessage = "请输入有效的电话号码")]
        public string ContactPhone { get; set; } = string.Empty;

        /// <summary>身份证号</summary>
        [StringLength(18, ErrorMessage = "身份证号长度必须为18位", MinimumLength = 18)]
        public string IdNumber { get; set; } = string.Empty;

        /// <summary>是否为传染病</summary>
        public bool IsInfectiousDisease { get; set; }

        /// <summary>发病时间</summary>
        public DateTime? DiseaseOnsetTime { get; set; }

        // ===================== 来自：DoctorClinic =====================

        /// <summary>主治医生ID</summary>
        [Required(ErrorMessage = "主治医生ID不能为空！")]
        public Guid DoctorId { get; set; }

        /// <summary>门诊科室名称</summary>
        [Required(ErrorMessage = "门诊科室不能为空！")]
        [StringLength(50, ErrorMessage = "门诊科室名称不能超过50个字符！")]
        public string DepartmentName { get; set; } = string.Empty;

        /// <summary>就诊类型（初诊/复诊）</summary>
        [Required(ErrorMessage = "就诊类型不能为空")]
        [StringLength(20, ErrorMessage = "就诊类型长度不能超过20个字符")]
        public string VisitType { get; set; } = "初诊";

        /// <summary>就诊时间（如果没有则使用当前时间）</summary>
        public DateTime? VisitDate { get; set; }

        /// <summary>备注信息</summary>
        [StringLength(1000, ErrorMessage = "备注信息不能超过1000个字符！")]
        public string? Remarks { get; set; }

        // ===================== 来自：Sick =====================

        /// <summary>体温（℃）</summary>
        [Required]
        [Range(30, 45, ErrorMessage = "体温应在30~45℃之间")]
        public decimal Temperature { get; set; }

        /// <summary>脉搏（次/min）</summary>
        [Required]
        [Range(20, 200, ErrorMessage = "脉搏应在20~200次/min之间")]
        public int Pulse { get; set; }

        /// <summary>呼吸（次/min）</summary>
        [Required]
        [Range(5, 60, ErrorMessage = "呼吸应在5~60次/min之间")]
        public int Breath { get; set; }

        /// <summary>血压（mmHg）</summary>
        [Required]
        [StringLength(16)]
        public string BloodPressure { get; set; } = string.Empty;

        // ===================== 内部定义：就诊状态枚举 =====================

        /// <summary>
        /// 就诊状态【1】待审核【2】已退回【3】已撤回【4】待接诊【5】已取消【6】已街镇【7】待随访【8】待评价
        /// </summary>
        public enum ExecutionStatus
        {
            PendingReview = 1,
            Returned = 2,
            Withdrawn = 3,
            PendingConsultation = 4,
            Cancelled = 5,
            Completed = 6,
            PendingFollowUp = 7,
            PendingEvaluation = 8
        }
    }

}
