using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Smart_Medical.Until;
using Microsoft.AspNetCore.Mvc;

namespace Smart_Medical.Pharmacy
{
    /// <summary>
    /// 制药公司服务实现
    /// </summary>
    public class PharmaceuticalCompanyAppService : ApplicationService, IPharmaceuticalCompanyAppService
    {
        private readonly IRepository<PharmaceuticalCompany, Guid> _repository;

        public PharmaceuticalCompanyAppService(IRepository<PharmaceuticalCompany, Guid> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 根据公司名称查询
        /// </summary>
        /// <param name="name">公司名称</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> FindByNameAsync(string name)
        {
            try
            {
                // 从数据库中获取包含指定名称的公司列表
                var companies = await _repository.GetListAsync(c => c.CompanyName.Contains(name));

                if (companies!=null)
                {
                    return ApiResult.Success(ResultCode.NotFound);
                }

                var result = ObjectMapper.Map<List<PharmaceuticalCompany>, List<PharmaceuticalCompanyDto>>(companies);
                return ApiResult<List<PharmaceuticalCompanyDto>>.Success(result, ResultCode.Success);
            }
            catch (Exception ex)
            {
                return ApiResult.Fail($"查询公司失败: {ex.Message}", ResultCode.Error);
            }
        }

        /// <summary>
        /// 获取所有公司列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> GetListAllAsync()
        {
            try
            {
                // 获取所有公司列表
                var companies = await _repository.GetListAsync();

                if (companies!=null)
                {
                    return ApiResult.Success(ResultCode.NotFound);
                }

                var result = ObjectMapper.Map<List<PharmaceuticalCompany>, List<PharmaceuticalCompanyDto>>(companies);
                return ApiResult<List<PharmaceuticalCompanyDto>>.Success(result, ResultCode.Success);
            }
            catch (Exception ex)
            {
                return ApiResult.Fail($"获取公司列表失败: {ex.Message}", ResultCode.Error);
            }
        }
    }
}
