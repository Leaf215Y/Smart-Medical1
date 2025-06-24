using Microsoft.AspNetCore.Mvc;
using Smart_Medical.DoctorvVsit.DockerDepartments;
using Smart_Medical.Until;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Smart_Medical.DoctorvVsit
{
    /// <summary>
    /// 科室管理
    /// </summary>
    [ApiExplorerSettings(GroupName = "科室管理")]
    public class DoctorDepartmentService : ApplicationService, IDoctorDepartmentService
    {
        private readonly IRepository<DoctorDepartment, Guid> dept;

        public DoctorDepartmentService(IRepository<DoctorDepartment, Guid> dept)
        {
            this.dept = dept;
        }
        /// <summary>
        /// 新增科室
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> InsertDoctorDepartment(CreateUpdateDoctorDepartmentDto input)
        {
            var deptdto = ObjectMapper.Map<CreateUpdateDoctorDepartmentDto, DoctorDepartment>(input);
            deptdto = await dept.InsertAsync(deptdto);
            return ApiResult.Success(ResultCode.Success);
        }
        /// <summary>
        /// 获取科室列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<PageResult<List<GetDoctorDepartmentListDto>>>> GetDoctorDepartmentList([FromQuery] GetDoctorDepartmentSearchDto search)
        {
            var list = await dept.GetQueryableAsync();
            list = list.WhereIf(!string.IsNullOrEmpty(search.DepartmentName), x => x.DepartmentName.Contains(search.DepartmentName));
            var res = list.PageResult(search.PageIndex, search.PageSize);
            var dto = ObjectMapper.Map<List<DoctorDepartment>, List<GetDoctorDepartmentListDto>>(res.Queryable.ToList());
            var pageInfo = new PageResult<List<GetDoctorDepartmentListDto>>
            {
                Data = dto,
                TotleCount = res.RowCount,
                TotlePage = (int)Math.Ceiling((double)res.RowCount / search.PageSize),
            };

            return ApiResult<PageResult<List<GetDoctorDepartmentListDto>>>.Success(pageInfo, ResultCode.Success);
        }
        /// <summary>
        /// 修改科室列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult> UpdateDoctorDepartment(Guid id, CreateUpdateDoctorDepartmentDto input)
        {
            var deptlist = await dept.FindAsync(id);
            if (deptlist.DepartmentName == input.DepartmentName)
            {
                return ApiResult.Fail("科室名称已存在不能修改", ResultCode.NotFound);
            }
            var dto = ObjectMapper.Map(input, deptlist);
            await dept.UpdateAsync(dto);
            return ApiResult.Success(ResultCode.Success);
        }
        /// <summary>
        /// 删除科室信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult> DeleteDoctorDepartment(Guid id)
        {
            var deptlist = await dept.FindAsync(id);
            await dept.DeleteAsync(deptlist);
            return ApiResult.Success(ResultCode.Success);
        }
    }
}
