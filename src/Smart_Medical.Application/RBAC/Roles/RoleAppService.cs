using Microsoft.AspNetCore.Mvc;
using Smart_Medical.RBAC.Users;
using Smart_Medical.Until;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace Smart_Medical.RBAC.Roles
{
    public class RoleAppService : ApplicationService, IRoleAppService
    {
        private readonly IRepository<Role, int> role;

        public RoleAppService(IRepository<Role, int> role)
        {
            this.role = role;
        }
        public async Task<ApiResult> CreateAsync(CreateUpdateRoleDto input)
        {
            var roles = ObjectMapper.Map<CreateUpdateRoleDto, Role>(input);
            var result = await role.InsertAsync(roles);
            return ApiResult.Success(ResultCode.Success);
        }

        public async Task<PageResult<List<RoleDto>>> GetListAsync([FromQuery] Seach seach)
        {
            var list = await role.GetListAsync();

            var totalCount = list.Count;
            var totalPage = (int)Math.Ceiling((double)totalCount / seach.PageSize);
            var pagedList = list.Skip((seach.PageIndex - 1) * seach.PageSize).Take(seach.PageSize).ToList();
            var userDtos = ObjectMapper.Map<List<Role>, List<RoleDto>>(pagedList);

            return new PageResult<List<RoleDto>>
            {
                TotleCount = totalCount,
                TotlePage = totalPage,
                Data = userDtos
            };
        }
    }
}
