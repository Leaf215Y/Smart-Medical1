using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Smart_Medical.RBAC
{
    /// <summary>
    /// 角色实体类
    /// 继承自 FullAuditedAggregateRoot<Guid>，表示这是一个具有完整审计功能（创建/修改/删除时间、操作人）的聚合根实体，主键类型为Guid。
    /// </summary>
    public class Role : FullAuditedAggregateRoot<Guid>
    {
        [Required(ErrorMessage = "角色名称是必填项。")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "用户名长度必须在 3 到 50 个字符之间。")]
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }


        [Required(ErrorMessage = "角色描述是必填项。")]
        [StringLength(200, ErrorMessage = "角色描述不能超过200个字符！")]
        /// <summary>
        /// 角色描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 角色和用户之间的多对多关联导航属性。
        /// ICollection<UserRole> 表示一个角色可以被多个用户通过 UserRole 关联。
        /// 这是一个集合导航属性，用于从 Role 实体导航到与之关联的所有 UserRole 实体。
        /// </summary>
        public ICollection<UserRole> UserRoles { get; set; }
        /// <summary>
        /// 角色和权限之间的多对多关联导航属性。
        /// ICollection<RolePermission> 表示一个角色可以拥有多个权限通过 RolePermission 关联。
        /// 这是一个集合导航属性，用于从 Role 实体导航到与之关联的所有 RolePermission 实体。
        /// </summary>
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
