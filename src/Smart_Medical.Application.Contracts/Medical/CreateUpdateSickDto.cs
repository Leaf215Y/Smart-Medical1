using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Medical.Medical
{
    public class CreateUpdateSickDto
    {
        /// <summary>
        /// 患者基本信息Id
        /// </summary>
        [Required(ErrorMessage = "患者基本信息Id不能为空")]
        public Guid BasicPatientId { get; set; }

        /// <summary>
        /// 病历状态
        /// 例如：住院中、已出院、已结算等
        /// </summary>
        [Required(ErrorMessage = "病历状态不能为空")]
        [StringLength(32, ErrorMessage = "病历状态长度不能超过32个字符")]
        public string Status { get; set; }

<<<<<<< Updated upstream
        /// <summary>
        /// 患者姓名（必填）
        /// </summary>
        [Required(ErrorMessage = "患者姓名不能为空")]
        [StringLength(50, ErrorMessage = "患者姓名长度不能超过50个字符")]
        public string PatientName { get; set; } = string.Empty;
=======

>>>>>>> Stashed changes

        /// <summary>
        /// 体温
        /// 单位：摄氏度（℃）
        /// 正常范围：30-45℃
        /// </summary>
        [Required(ErrorMessage = "体温不能为空")]
        [Range(30, 45, ErrorMessage = "体温必须在30~45℃之间")]
        public decimal Temperature { get; set; }

        /// <summary>
        /// 脉搏
        /// 单位：次/分钟
        /// 正常范围：20-200次/分钟
        /// </summary>
        [Required(ErrorMessage = "脉搏不能为空")]
        [Range(20, 200, ErrorMessage = "脉搏必须在20~200次/分钟之间")]
        public int Pulse { get; set; }

        /// <summary>
        /// 呼吸频率
        /// 单位：次/分钟
        /// 正常范围：5-60次/分钟
        /// </summary>
        [Required(ErrorMessage = "呼吸频率不能为空")]
        [Range(5, 60, ErrorMessage = "呼吸频率必须在5~60次/分钟之间")]
        public int Breath { get; set; }

        /// <summary>
        /// 血压
        /// 格式：收缩压/舒张压，单位：mmHg
        /// 例如：120/80
        /// </summary>
        [Required(ErrorMessage = "血压不能为空")]
        [StringLength(16, ErrorMessage = "血压长度不能超过16个字符")]
        public string BloodPressure { get; set; }


        /// <summary>
        /// 出院诊断
        /// 记录病人出院时的最终诊断结果
        /// </summary>
        //[Required(ErrorMessage = "出院诊断不能为空")]
        [StringLength(128, ErrorMessage = "出院诊断长度不能超过128个字符")]
        public string? DischargeDiagnosis { get; set; } = string.Empty;
        /// <summary>
        /// 住院号
        /// 唯一标识病人的住院记录，不可重复
        /// </summary>
        //[Required(ErrorMessage = "住院号不能为空")]
        [StringLength(32, ErrorMessage = "住院号长度不能超过32个字符")]
        public string? InpatientNumber { get; set; } = string.Empty;
        /// <summary>
        /// 出院科室
        /// 记录病人最后所在的科室
        /// </summary>
        [Required(ErrorMessage = "出院科室不能为空")]
        [StringLength(64, ErrorMessage = "出院科室长度不能超过64个字符")]
        public string? DischargeDepartment { get; set; } = string.Empty;
        /// <summary>
        /// 出院时间
        /// 记录病人实际离院的时间
        /// 不能早于当前时间
        /// </summary>
        //[Required(ErrorMessage = "出院时间不能为空")]
        public DateTime? DischargeTime { get; set; } = null;
        /// <summary>
        /// 入院诊断
        /// 记录病人入院时的初步诊断结果
        /// </summary>
        //[Required(ErrorMessage = "入院诊断不能为空")]
        [StringLength(128, ErrorMessage = "入院诊断长度不能超过128个字符")]
        public string? AdmissionDiagnosis { get; set; } = string.Empty;

    }
}
