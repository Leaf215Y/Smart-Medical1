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
        /// </summary>
        /// <param name="name">公司名称</param>
        /// <returns></returns>
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
        /// </summary>
        /// <returns></returns>
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
