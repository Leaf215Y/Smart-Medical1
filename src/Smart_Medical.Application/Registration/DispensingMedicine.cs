using Microsoft.Extensions.Logging;
using Smart_Medical.DoctorvVsit;
using Smart_Medical.Medical;
using Smart_Medical.Patient;
using Smart_Medical.Pharmacy;
using Smart_Medical.Until;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace Smart_Medical.Registration
{
    /// <summary>
    /// 收费发药
    /// </summary>
    public class DispensingMedicine : ApplicationService
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        /// <summary>
        /// 就诊流程
        /// </summary>
        private readonly IRepository<DoctorClinic, Guid> _doctorclinRepo;
        /// <summary>
        /// 患者基本信息
        /// </summary>
        private readonly IRepository<BasicPatientInfo, Guid> _patientRepo;
        /// <summary>
        /// 患者病历信息
        /// </summary>
        private readonly IRepository<Sick, Guid> _sickRepo;
        /// <summary>
        /// 患者开具处方
        /// </summary>
        private readonly IRepository<PatientPrescription, Guid> _prescriptionRepo;
        /// <summary>
        /// 药品
        /// </summary>
        private readonly IRepository<Drug, int> _drugRepo;
        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="unitOfWorkManager"></param>
        /// <param name="doctorclinRepo"></param>
        /// <param name="basicpatientRepo"></param>
        /// <param name="sickRepo"></param>
        /// <param name="prescriptionRepo"></param>
        public DispensingMedicine(
            IUnitOfWorkManager unitOfWorkManager, IRepository<DoctorClinic, Guid> doctorclinRepo, IRepository<BasicPatientInfo, Guid> basicpatientRepo, IRepository<Sick, Guid> sickRepo, IRepository<PatientPrescription, Guid> prescriptionRepo, IRepository<Drug,int> drugRepo)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _doctorclinRepo = doctorclinRepo;
            _patientRepo = basicpatientRepo;
            _sickRepo = sickRepo;
            _prescriptionRepo = prescriptionRepo;
            _drugRepo = drugRepo;
        }

        /// <summary>
        /// 发药接口，根据患者编号统一处理患者的所有处方药品发药逻辑
        /// </summary>
        /// <param name="patientNumber">患者编号（GUID）</param>
        /// <returns>返回 ApiResult，标识发药成功或失败及相关提示信息</returns>
        public async Task<ApiResult> DistributeMedicine(Guid patientNumber)
        {
            // 先来个“身份证”校验，患者编号不能是空Guid，空Guid你发啥药？
            if (patientNumber == Guid.Empty)
            {
                return ApiResult.Fail("患者编号不能为空！", ResultCode.NotFound);
            }

            // 开启一个新的事务单元
            using (var uow = _unitOfWorkManager.Begin(requiresNew: true))
            {
                try
                {
                    //// 从处方仓库拉取该患者所有处方数据
                    //var prescriptions = await _prescriptionRepo.GetListAsync(x => x.PatientNumber == patientNumber);

                    //// 如果没处方，直接闪人，不给发药，省得给个空瓶子回家
                    //if (!prescriptions.Any())
                    //{
                    //    return ApiResult.Fail("该患者没有处方，无法发药！", ResultCode.NotFound);
                    //}

                    //// 把患者所有处方中涉及的药品名都挑出来，去重，准备查库存
                    //var drugNames = prescriptions.Select(p => p.MedicationName).Distinct().ToList();

                    //// 根据药品名称批量查询药品库存数据，别一条条查，效率低得想砸电脑
                    //var drugs = await _drugRepo.GetListAsync(d => drugNames.Contains(d.DrugName));

                    //// 先过一遍库存关，药品得存在且库存够用才能发，否则咱们也没法“送药上门”
                    //foreach (var pres in prescriptions)
                    //{
                    //    var drug = drugs.FirstOrDefault(d => d.DrugName == pres.MedicationName);
                    //    if (drug == null)
                    //    {
                    //        return ApiResult.Fail($"药品[{pres.MedicationName}]不存在库存记录，无法发药！", ResultCode.NotFound);
                    //    }
                    //    if (drug.Stock < pres.Number)
                    //    {
                    //        return ApiResult.Fail($"药品[{pres.MedicationName}]库存不足，当前库存：{drug.Stock}，需求数量：{pres.Number}", ResultCode.NotFound);
                    //    }
                    //}

                    //// 库存没问题，准备扣减库存
                    //foreach (var pres in prescriptions)
                    //{
                    //    var drug = drugs.First(d => d.DrugName == pres.MedicationName);
                    //    drug.Stock -= pres.Number; // 库存减少对应处方药数量
                    //    await _drugRepo.UpdateAsync(drug); // 记得及时同步更新数据库，不然药品“凭空消失”就麻烦了
                    //}

                    //// 找患者对应的就诊流程记录（流程表）
                    //var clinic = await _doctorclinRepo.FirstOrDefaultAsync(x => x.PatientId == patientNumber);

                    //if (clinic == null)
                    //{
                    //    return ApiResult.Fail("不存在此流程", ResultCode.NotFound);
                    //}

                    //// 发药完成后，更新就诊流程的发药状态为1（已发药）
                    //clinic.DispensingStatus = 1;
                    //await _doctorclinRepo.UpdateAsync(clinic);

                    //// 全流程成功，提交事务，别让数据“玩消失”
                    //await uow.CompleteAsync();

                    //// 全部顺利，发药成功，回家煮碗面庆祝吧
                    return ApiResult.Success(ResultCode.Success);
                }
                catch (Exception ex)
                {
                    // 发生异常，先别慌，记录日志方便以后查谁干的好事
                    Logger.LogException(ex);

                    // 返回系统异常提示，别给客户解释“臭鱼烂虾”之类的
                    return ApiResult.Fail("发药失败，系统异常！", ResultCode.NotFound);
                }
            }

        }
    }

}

