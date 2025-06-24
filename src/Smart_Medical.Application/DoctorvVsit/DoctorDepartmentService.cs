using Microsoft.AspNetCore.Mvc;
using Polly.Caching;
using Smart_Medical.DoctorvVsit.DockerDepartments;
using Smart_Medical.Until;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace Smart_Medical.DoctorvVsit
{
    /// <summary>
    /// 科室管理
    /// </summary>
    public class DoctorDepartmentService: ApplicationService, IDoctorDepartmentService
    {
        private readonly IRepository<DoctorDepartment,Guid> dept;

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
            var deptdto=ObjectMapper.Map<CreateUpdateDoctorDepartmentDto, DoctorDepartment>(input);
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
            var list=await dept.GetQueryableAsync();
            list=list.WhereIf(!string.IsNullOrEmpty(search.DepartmentName),x=>x.DepartmentName.Contains(search.DepartmentName));
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
        public async Task<ApiResult> UpdateDoctorDepartment(Guid id,CreateUpdateDoctorDepartmentDto input)
        {
            var deptlist = await dept.FindAsync(id);
           //if(deptlist.DepartmentName==input.DepartmentName)
           // {
           //     return ApiResult.Fail("科室名称已存在不能修改", ResultCode.NotFound);
           // }
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

        /// <summary>
        /// 批量删除科室信息
        /// </summary>
        /// <param name="idsString">要删除的科室ID字符串，例如："guid1,guid2,guid3"</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult> DeleteDoctorDepartment([FromQuery] string idsString) // 参数类型改为string
        {
            if (string.IsNullOrWhiteSpace(idsString))
            {
                return ApiResult.Fail("请提供要删除的科室ID字符串。", ResultCode.NotFound);
            }

            // 将逗号分隔的字符串解析为 List<Guid>
            var ids = idsString.Split(',')
                               .Where(s => !string.IsNullOrWhiteSpace(s))
                               .Select(s =>
                               {
                                   if (Guid.TryParse(s.Trim(), out Guid id))
                                   {
                                       return id;
                                   }
                                   throw new FormatException($"无效的GUID格式: {s}"); // 如果有无效GUID，可以抛出异常
                               })
                               .ToList();

            if (!ids.Any())
            {
                return ApiResult.Fail("解析后的科室ID列表为空。", ResultCode.NotFound);
            }

            // 查找所有需要删除的科室实体

            var deptListToDelete = await dept.GetQueryableAsync();
            deptListToDelete=deptListToDelete.Where(d => ids.Contains(d.Id));
            if (!deptListToDelete.Any())
            {
                return ApiResult.Fail("没有找到匹配的科室信息。", ResultCode.NotFound);
            }

            // 批量删除 (硬删除或软删除取决于您的实体是否实现ISoftDelete)
            await dept.DeleteManyAsync(deptListToDelete);

            return ApiResult.Success(ResultCode.Success);
        }

    }
}
