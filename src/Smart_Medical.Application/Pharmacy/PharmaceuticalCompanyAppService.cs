using AutoMapper.Internal.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Smart_Medical.Pharmacy
{
    public class PharmaceuticalCompanyAppService :
       CrudAppService<
           PharmaceuticalCompany,
           PharmaceuticalCompanyDto,
           Guid,
           PagedAndSortedResultRequestDto,
           CreateUpdatePharmaceuticalCompanyDto>,
       IPharmaceuticalCompanyAppService
    {
        public PharmaceuticalCompanyAppService(IRepository<PharmaceuticalCompany, Guid> repository)
            : base(repository)
        {

        }

        /// <summary>
        /// 根据公司名称模糊查询
        /// Search pharmaceutical companies by name (fuzzy search)
        /// </summary>
        /// <remarks>
        /// GET /api/app/pharmacy/pharmaceutical-company/find-by-name?name={name}
        /// 根据公司名称模糊查询医药公司信息。
        /// </remarks>
        /// <param name="name">公司名称（模糊查询）</param>
        /// <returns>公司列表</returns>
        public async Task<ListResultDto<PharmaceuticalCompanyDto>> FindByNameAsync(string name)
        {
            //从仓储中获取包含指定名称的公司列表
            var companies = await Repository.GetListAsync(c => c.CompanyName.Contains(name));
            //将实体列表映射到DTO列表并返回
            return new ListResultDto<PharmaceuticalCompanyDto>(
                ObjectMapper.Map<List<PharmaceuticalCompany>, List<PharmaceuticalCompanyDto>>(companies)
            );
        }

        /// <summary>
        /// 获取所有公司列表
        /// Get all pharmaceutical companies
        /// </summary>
        /// <remarks>
        /// GET /api/app/pharmacy/pharmaceutical-company/all
        /// 获取所有医药公司信息列表。
        /// </remarks>
        /// <returns>公司列表</returns>
        public async Task<ListResultDto<PharmaceuticalCompanyDto>> GetListAllAsync()
        {
            //从仓储中获取所有公司
            var companies = await Repository.GetListAsync();
            //将实体列表映射到DTO列表并返回
            return new ListResultDto<PharmaceuticalCompanyDto>(
                ObjectMapper.Map<List<PharmaceuticalCompany>, List<PharmaceuticalCompanyDto>>(companies)
            );
        }
    }
}
