using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Smart_Medical.Until;
using System.Threading.Tasks;
using Smart_Medical.Application.Contracts.RBAC.Users;

namespace Smart_Medical.RBAC.Users
{
    /// <summary>
    /// 用户接口服务
    /// </summary>
    public interface IUserAppService : IApplicationService
    {
        Task<ApiResult<UserDto>> GetAsync(Guid id);
        Task<ApiResult<PageResult<List<UserDto>>>> GetListAsync(SeachUserDto input);
        Task<ApiResult> InsertUserPTAsync(CreateUpdateUserDto input);
        Task<ApiResult> UpdateAsync(Guid id, CreateUpdateUserDto input);
        Task<ApiResult> DeleteAsync(Guid id);
        Task<ApiResult<UserDto>> GetByUserNameAsync(string username);
        Task<ApiResult<UserDto>> LoginAsync(LoginDto loginDto);
        Task<ApiResult> AddPatientInfoAsync(Guid userId, AddPatientInfoDto input);
    }
}
