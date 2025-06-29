using Microsoft.AspNetCore.Mvc;
using Smart_Medical.Application.Contracts.RBAC.Permissions;
using Smart_Medical.Enums;
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
        /// 获取菜单权限树（仅支持两级菜单结构）
        /// </summary>
        /// <param name="parentId">可选，父级菜单Id，当前未用到</param>
        /// <returns>菜单权限树列表</returns>
        public async Task<ApiResult<List<GetMenuPermissionTree>>> GetMenuPermissionTreeList(Guid? parentId=null)
        {
            // 1. 获取所有权限数据的 IQueryable
            var permissionList = await _permissionRepository.GetQueryableAsync();

            // 2. 将所有权限数据加载到内存，避免后续多次数据库访问
            var permissionEntityList = permissionList.ToList();

            // 3. 过滤出所有顶级菜单（ParentId==null）
            var menuPermissionList = permissionEntityList
                .Where(x => x.ParentId == null)
                .Select(p => new GetMenuPermissionTree
                {
                    Id = p.Id,
                    PermissionName = p.PermissionName,
                    PermissionCode = p.PermissionCode,
                    Type = p.Type,
                    PagePath = p.PagePath,
                    ParentId = p.ParentId,
                    // 4. 查找该顶级菜单下的所有子菜单（ParentId==当前菜单Id，且&& x.Type == PermissionType.Menu）
                    Children = permissionEntityList
                        .Where(x => x.ParentId == p.Id && x.Type == PermissionType.Menu)
                        .Select(x => new GetMenuPermissionTree
                        {
                            Id = x.Id,
                            PermissionName = x.PermissionName,
                            PermissionCode = x.PermissionCode,
                            Type = x.Type,
                            PagePath = x.PagePath,
                            ParentId = x.ParentId,
                            Children= new List<GetMenuPermissionTree>()
                            //    Children = permissionEntityList
                            //.Where(c => c.ParentId == x.Id)
                            //.Select(c => new GetMenuPermissionTree
                            //{
                            //    Id = c.Id,
                            //    PermissionName = c.PermissionName,
                            //    PermissionCode = c.PermissionCode,
                            //    Type = c.Type,
                            //    PagePath = c.PagePath,
                            //    ParentId = c.ParentId,
                            //    Children = new List<GetMenuPermissionTree>()
                            //}).ToList()
                        }).ToList()
                }).ToList();

            // 5. 返回树形结构结果
            return ApiResult<List<GetMenuPermissionTree>>.Success(menuPermissionList, ResultCode.Success);
        }
    }
}