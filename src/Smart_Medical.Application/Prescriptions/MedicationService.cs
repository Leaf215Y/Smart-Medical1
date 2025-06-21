using Microsoft.AspNetCore.Mvc;
using Smart_Medical.Until;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Smart_Medical.Prescriptions
{
    public class MedicationService: ApplicationService, IMedicationService
    {
        private readonly IRepository<Medication> medica;

        public MedicationService(IRepository<Medication> medica)
        {
            this.medica = medica;
        }
        /// <summary>
        /// 创建每个处方下的用药药品
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
        public async Task<ApiResult<PageResult<List<MedicationDto>>>> GetMedicationList(int PrescriptionId,int pageIndex,int pageSize)
        {

            var res = await medica.GetListAsync(x => x.PrescriptionId == PrescriptionId);
            var totalCount =await medica.CountAsync(x => x.PrescriptionId == PrescriptionId);
            var totalPage = (int)Math.Ceiling((double)totalCount / pageSize);
           
            res=res.OrderByDescending(x=>x.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            var dto = ObjectMapper.Map<List<Medication>, List<MedicationDto>>(res);
            return ApiResult<PageResult<List<MedicationDto>>>.Success(new PageResult<List<MedicationDto>> { Data=dto,}, ResultCode.Success);
        }
    }
}
