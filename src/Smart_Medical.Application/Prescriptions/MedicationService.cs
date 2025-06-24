using Microsoft.AspNetCore.Mvc;
using Smart_Medical.Until;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace Smart_Medical.Prescriptions
{
    /// <summary>
    /// 处方下对应用药管理
    /// </summary>
    //[ApiExplorerSettings(GroupName = "处方下对应用药管理")]
    public class MedicationService: ApplicationService, IMedicationService
    {
        private readonly IRepository<Medication> medica;

        public MedicationService(IRepository<Medication> medica)
        {
            this.medica = medica;
        }
        /// <summary>
        /// 新增每个处方下的用药药品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> CreateAsync(CreateUpdateMedicationDto input)
        {
            var res = ObjectMapper.Map<CreateUpdateMedicationDto, Medication>(input);
            res = await medica.InsertAsync(res);

            return ApiResult.Success(ResultCode.Success);
        }
        /// <summary>
        /// 获取每个处方下的所有用药药品
        /// </summary>
        /// <param name="PrescriptionId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<PageResult<List<MedicationDto>>>> GetMedicationList([FromQuery]MedicationSearchDto search)
        {
           
            var list = await medica.GetQueryableAsync();
            list = list.WhereIf(search.PrescriptionId!=null,x=>x.PrescriptionId==search.PrescriptionId);
           
            var res = list.PageResult(search.pageIndex, search.pageSize);
            var dto = ObjectMapper.Map<List<Medication>, List<MedicationDto>>(res.Queryable.ToList());
            var pageInfo = new PageResult<List<MedicationDto>>
            {
                Data = dto,
                TotleCount = res.RowCount,
                TotlePage = (int)Math.Ceiling((double)res.RowCount / search.pageSize)
            };
            return ApiResult<PageResult<List<MedicationDto>>>.Success(pageInfo, ResultCode.Success);
        }
    }
}
