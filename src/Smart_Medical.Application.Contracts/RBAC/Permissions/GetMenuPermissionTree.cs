using Smart_Medical.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Medical.RBAC.Permissions
{
    /// <summary>
    /// 获取菜单权限树
    /// </summary>
    public class GetMenuPermissionTree
    {
        public Guid Id { get; set; }
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
        public Guid? ParentId { get; set; } = null;

        public IList<GetMenuPermissionTree> Children { get; set; }=new List<GetMenuPermissionTree>();

        /// <summary>
        /// 自定义属性
        /// </summary>
        public string? Icon { get; set; } = string.Empty;
    }
}
