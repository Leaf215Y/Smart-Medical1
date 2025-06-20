using Microsoft.AspNetCore.Mvc;
using Smart_Medical.Until;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Smart_Medical.RBAC.Users
{
    public interface IUserAppService : IApplicationService
    {
        Task<ApiResult<UserDto>> GetAsync(Guid id);
        Task<PageResult<List<UserDto>>> GetListAsync([FromQuery] Seach seach);
        Task<ApiResult> CreateAsync(CreateUpdateUserDto input);
        Task<ApiResult> UpdateAsync(Guid id, CreateUpdateUserDto input);
        Task<ApiResult> DeleteAsync(Guid id);
        Task<ApiResult<UserDto>> LoginAsync(LoginDto loginDto);
    }
}
