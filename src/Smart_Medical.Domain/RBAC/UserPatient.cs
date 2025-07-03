using Smart_Medical.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Smart_Medical.RBAC
{
    /// <summary>
    /// 用户-患者关联表
    /// </summary>
    public class UserPatient : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 导航属性：关联的用户
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// 患者Id
        /// </summary>
        public Guid PatientId { get; set; }

        /// <summary>
        /// 导航属性：关联的患者
        /// </summary>
        public BasicPatientInfo Patient { get; set; }

        protected UserPatient()
        {
        }

        public UserPatient(Guid id) : base(id)
        {
        }
    }
}
