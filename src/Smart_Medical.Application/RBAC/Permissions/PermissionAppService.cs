using Microsoft.AspNetCore.Mvc;
using Smart_Medical.Application.Contracts.RBAC.Permissions;
using Smart_Medical.Until;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Smart_Medical.RBAC.Permissions
{
    [ApiExplorerSettings(GroupName = "权限管理")]
    public class PermissionAppService : ApplicationService, IPermissionAppService
    {
        private readonly IRepository<Permission, Guid> _permissionRepository;

        public PermissionAppService(IRepository<Permission, Guid> permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }
        /// <summary>
        /// 创建权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ApiResult> CreateAsync(CreateUpdatePermissionDto input)
        {
            var permission = ObjectMapper.Map<CreateUpdatePermissionDto, Permission>(input);
            await _permissionRepository.InsertAsync(permission);
            return ApiResult.Success(ResultCode.Success);
        }
        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResult> DeleteAsync(Guid id)
        {
            var permission = await _permissionRepository.GetAsync(id);
            if (permission == null)
            {
                return ApiResult.Fail("权限不存在", ResultCode.NotFound);
            }
            await _permissionRepository.DeleteAsync(permission);
            return ApiResult.Success(ResultCode.Success);
        }
        /// <summary>
        /// 根据Id获取权限信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResult<PermissionDto>> GetAsync(Guid id)
        {
            var permission = await _permissionRepository.GetAsync(id);
            if (permission == null)
            {
                return ApiResult<PermissionDto>.Fail("权限不存在", ResultCode.NotFound);
            }
            var permissionDto = ObjectMapper.Map<Permission, PermissionDto>(permission);
            return ApiResult<PermissionDto>.Success(permissionDto, ResultCode.Success);
        }
        /// <summary>
        /// 获取权限列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ApiResult<PageResult<List<PermissionDto>>>> GetListAsync([FromQuery] SeachPermissionDto input)
        {
            var queryable = await _permissionRepository.GetQueryableAsync();

            if (!string.IsNullOrWhiteSpace(input.PermissionName))
            {
                queryable = queryable.Where(p => p.PermissionName.Contains(input.PermissionName));
            }

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            queryable = queryable
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .OrderBy(p => p.PermissionName); // 默认排序

            var permissions = await AsyncExecuter.ToListAsync(queryable);
            var permissionDtos = ObjectMapper.Map<List<Permission>, List<PermissionDto>>(permissions);

            var pageResult = new PageResult<List<PermissionDto>>
            {
                TotleCount = totalCount,
                TotlePage = (int)Math.Ceiling((double)totalCount / input.MaxResultCount),
                Data = permissionDtos
            };

            return ApiResult<PageResult<List<PermissionDto>>>.Success(pageResult, ResultCode.Success);
        }
        /// <summary>
        /// 修改权限详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ApiResult> UpdateAsync(Guid id, CreateUpdatePermissionDto input)
        {
            var permission = await _permissionRepository.GetAsync(id);
            if (permission == null)
            {
                return ApiResult.Fail("权限不存在", ResultCode.NotFound);
            }

            ObjectMapper.Map(input, permission);
            await _permissionRepository.UpdateAsync(permission);
            return ApiResult.Success(ResultCode.Success);
        }
        /// <summary>
        /// 获取菜单权限树
        /// </summary>
        /// <returns></returns>
        //public async Task<ApiResult<IList<GetMenuPermissionTree>>> GetMenuPermissionTreeList(Guid parentId)
        //{
        //    var permissionList = await _permissionRepository.GetQueryableAsync();
        //    permissionList= permissionList.Where(p => p.ParentId == null);
        //    var menuPermissionList = permissionList.Select(async p => new GetMenuPermissionTree
        //    {
        //        Id = p.Id,
        //        PermissionName = p.PermissionName,
        //        PermissionCode = p.PermissionCode,
        //        Type = 0,
        //        PagePath = p.PagePath,
        //        ParentId = p.ParentId,
        //        Children = _permissionRepository.GetQueryableAsync()
        //    }).ToList();


        //}
    }
}