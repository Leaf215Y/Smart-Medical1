using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Smart_Medical.RBAC
{
    /// <summary>
    /// 用户-角色关联实体类（中间表）
    /// 继承自 FullAuditedAggregateRoot<Guid>，表示这是一个具有完整审计功能的聚合根实体，主键类型为Guid。
    /// 该实体用于表示用户和角色之间的多对多关系。
    /// </summary>
    public class UserRole : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 用户Id，作为外键，关联到 User 实体。
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 角色Id，作为外键，关联到 Role 实体。
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// 导航属性：关联的用户实体。
        /// 用于从 UserRole 实体导航到其对应的 User 实体。
        /// </summary>
        public User User { get; set; }
        /// <summary>
        /// 导航属性：关联的角色实体。
        /// 用于从 UserRole 实体导航到其对应的 Role 实体。
        /// </summary>
        public Role Role { get; set; }

        public UserRole()
        {
            
        }

        public UserRole(Guid ID) : base()
        {
        }
    }
}
