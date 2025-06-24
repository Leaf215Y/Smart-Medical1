using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Smart_Medical.Pharmacy
{
    /// <summary>
    /// 制药公司服务接口
    /// </summary>
    public interface IPharmaceuticalCompanyAppService :
         ICrudAppService<
             PharmaceuticalCompanyDto,
             Guid,
             PagedAndSortedResultRequestDto,
             CreateUpdatePharmaceuticalCompanyDto>
    {
        /// <summary>
        /// 根据公司名称模糊查询
        /// </summary>
        /// <param name="name">公司名称</param>
        /// <returns></returns>
        Task<ListResultDto<PharmaceuticalCompanyDto>> FindByNameAsync(string name);

        /// <summary>
        /// 获取所有公司列表
        /// </summary>
        /// <returns></returns>
        Task<ListResultDto<PharmaceuticalCompanyDto>> GetListAllAsync();
    }
}
