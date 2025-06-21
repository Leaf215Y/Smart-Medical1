using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Medical.OutpatientClinic.Dtos
{
    /// <summary>
    /// 快速就诊、登记患者信息
    /// </summary>
    public class InsertPatientDto
    {
        #region  患者基本信息

        /// <summary>
        /// 患者姓名（必填）
        /// </summary>
        [Required(ErrorMessage = "患者姓名不能为空")]
        [StringLength(50, ErrorMessage = "患者姓名长度不能超过50个字符")]
        public string PatientName { get; set; } = string.Empty;

        /// <summary>
        /// 性别【1】男【2】女
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        [Range(0, 150, ErrorMessage = "年龄必须在0-150之间")]
        public int? Age { get; set; }

        /// <summary>
        /// 年龄单位（年/月/日）
        /// </summary>
        [StringLength(10, ErrorMessage = "年龄单位长度不能超过10个字符")]
        public string AgeUnit { get; set; } = string.Empty;

        /// <summary>
        /// 联系方式
        /// </summary>
        [StringLength(20, ErrorMessage = "联系方式长度不能超过20个字符")]
        [Phone(ErrorMessage = "请输入有效的电话号码")]
        public string ContactPhone { get; set; } = string.Empty;

        /// <summary>
        /// 身份证号
        /// </summary>
        [StringLength(18, ErrorMessage = "身份证号长度必须为18位", MinimumLength = 18)]
        public string IdNumber { get; set; } = string.Empty;

        /// <summary>
        /// 就诊类型（初诊/复诊，必填）
        /// </summary>
        [Required(ErrorMessage = "就诊类型不能为空")]
        [StringLength(20, ErrorMessage = "就诊类型长度不能超过20个字符")]
        public string VisitType { get; set; } = "初诊";

        /// <summary>
        /// 是否为传染病
        /// </summary>
        public bool IsInfectiousDisease { get; set; }

        /// <summary>
        /// 发病时间
        /// </summary>
        public DateTime? DiseaseOnsetTime { get; set; }

        #endregion

        #region  医生信息

        /// <summary>
        /// 所属科室编号
        /// </summary>
        public Guid DepartmentId { get; set; }

        /// <summary>
        /// 账户标识
        /// </summary>
        [Required(ErrorMessage = "账户标识不能为空")]
        [StringLength(20, ErrorMessage = "账户标识长度不能超过20个字符")]
        public string AccountId { get; set; } = string.Empty;

        /// <summary>
        /// 员工姓名
        /// </summary>
        [Required(ErrorMessage = "姓名不能为空")]
        [StringLength(20, ErrorMessage = "姓名长度不能超过20个字符")]
        public string EmployeeName { get; set; } = string.Empty;

        #endregion

        #region  病历



        #endregion

        #region 流程信息

        /// <summary>
        /// 主治医生ID 
        /// 关联到 DoctorAccount 实体，表示本次就诊的主要负责医生。
        /// </summary>
        [Required(ErrorMessage = "主治医生ID不能为空！")]
        public Guid DoctorId { get; set; }

        /// <summary>
        /// 就诊状态【1】待审核【2】已退回【3】已撤回【4】待接诊【5】已取消【6】已街镇【7】待随访【8】待评价
        /// </summary>
        public enum ExecutionStatus
        {
            PendingReview = 1, // 待审核
            Returned = 2, // 已退回
            Withdrawn = 3, // 已撤回
            PendingConsultation = 4, // 待接诊
            Cancelled = 5, // 已取消
            Completed = 6, // 已街镇
            PendingFollowUp = 7, // 待随访
            PendingEvaluation = 8 // 待评价
        }

        /// <summary>
        /// 备注信息 
        /// 任何需要补充的就诊相关说明或特殊情况。
        /// </summary>
        [StringLength(1000, ErrorMessage = "备注信息不能超过1000个字符！")]
        public string? Remarks { get; set; }

        #endregion
    }
}
