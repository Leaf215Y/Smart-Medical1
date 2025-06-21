using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Smart_Medical.Medical
{

    public class SickDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 病历状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 住院号
        /// </summary>
        public string InpatientNumber { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 出院科室
        /// </summary>
        public string DischargeDepartment { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 出院时间
        /// </summary>
        public DateTime DischargeTime { get; set; }

        /// <summary>
        /// 入院诊断
        /// </summary>
        public string AdmissionDiagnosis { get; set; }

        /// <summary>
        /// 出院诊断
        /// </summary>
        public string DischargeDiagnosis { get; set; }
    }
}
