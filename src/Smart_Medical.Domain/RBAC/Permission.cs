using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace Smart_Medical.RBAC
{
    /// <summary>
    /// 权限实体类
    /// 继承自 FullAuditedAggregateRoot<Guid>，表示这是一个具有完整审计功能（创建/修改/删除时间、操作人）的聚合根实体，主键类型为Guid。
    /// </summary>
    public class Permission : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 权限名称
        /// Required: 表示该字段是必填项。
        /// StringLength: 限制字符串长度在 3 到 50 个字符之间。
        /// </summary>
        [Required(ErrorMessage = "权限名称是必填项。")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "权限名称长度必须在 3 到 50 个字符之间。")]
        public string PermissionName { get; set; }


        /// <summary>
        /// 权限编码，唯一标识一个权限点，例如：UserManagement.AddUser, Product.View
        /// Required: 表示该字段是必填项。
        /// StringLength: 字符串长度在 3 到 100 个字符之间。
        /// </summary>
        [Required(ErrorMessage = "权限编码是必填项。")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "权限编码长度必须在 3 到 100 个字符之间。")]
        public string PermissionCode { get; set; }

        /// <summary>
        /// 权限类型（菜单、按钮、操作等）
        /// </summary>
        public PermissionType Type { get; set; }

        /// <summary>
        /// 对应的页面路径或路由地址 (仅当Type为Menu时有效)  只有菜单权限需要指定页面路径。
        /// </summary>
        [StringLength(200, ErrorMessage = "页面路径长度不能超过 200 个字符。")]
        public string PagePath { get; set; }

        /// <summary>
        /// 父级权限ID (用于构建菜单或权限的层级结构) 菜单权限为顶级权限 时可以为空。
        /// 可以为空，表示顶级权限。
        /// </summary>
        public Guid? ParentId { get; set; }=null;

        /// <summary>
        /// 权限和角色之间的多对多关联导航属性。
        /// ICollection<RolePermission> 表示一个权限可以被多个角色通过 RolePermission 关联。
        /// 这是一个集合导航属性，用于从 Permission 实体导航到与之关联的所有 RolePermission 实体。
        /// </summary>
        public ICollection<RolePermission> RolePermissions { get; set; }
    }

    // 权限类型枚举，用于区分权限是菜单、按钮还是其他类型
    public enum PermissionType
    {
        /// <summary>
        /// 菜单权限（用于控制菜单的显示）
        /// </summary>
        Menu = 0,

        /// <summary>
        /// 按钮权限（用于控制页面内按钮的显示和操作）
        /// </summary>
        Button = 1,

        /// <summary>
        /// 其他操作权限（例如API访问权限等）
        /// </summary>
        Operation = 2
    }
}
