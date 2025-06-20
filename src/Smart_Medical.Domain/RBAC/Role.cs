using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Smart_Medical.RBAC
{
    public class Role : FullAuditedAggregateRoot<int>
    {

        [Required(ErrorMessage = "用户名是必填项。")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "用户名长度必须在 3 到 50 个字符之间。")]
        public string RoleName { get; set; }

    }
}
