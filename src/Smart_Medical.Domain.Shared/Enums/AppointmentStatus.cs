using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Medical.Enums
{
    /// <summary>
    /// 挂号状态枚举
    /// </summary>
    public enum AppointmentStatus
    {
        Booked,      // 已预约
        Confirmed,   // 已确认 (例如，患者抵达医院)
        Completed,   // 已完成 (就诊结束)
        Canceled,    // 已取消
        NoShow,      // 未按时就诊
        Rescheduled  // 已改期
    }
}
