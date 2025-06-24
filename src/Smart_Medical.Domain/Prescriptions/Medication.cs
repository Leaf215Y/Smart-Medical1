using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Smart_Medical.Prescriptions
{
    /// <summary>
    /// 用药  删除
    /// </summary>
    public class Medication : FullAuditedAggregateRoot<Guid>
    {
       
    }
}
