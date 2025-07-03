using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Medical.RBAC.UserRolePermission
{
    public class UserRolePermissionDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
        public List<string> Permissions { get; set; }
    }

}
