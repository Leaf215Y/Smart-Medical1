using System;
using System.Threading.Tasks;
using Smart_Medical.Pharmacy;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Smart_Medical.Data
{
    public class DrugDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Drug, Guid> _drugRepository;

        public DrugDataSeedContributor(IRepository<Drug, Guid> drugRepository)
        {
            _drugRepository = drugRepository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (await _drugRepository.GetCountAsync() > 0)
            {
                return;
            }

            await _drugRepository.InsertAsync(new Drug
            {
                DrugName = "阿莫西林胶囊",
                DrugType = "抗生素",
                FeeName = "西药费",
                DosageForm = "胶囊剂",
                Specification = "0.25g*24粒",
                PurchasePrice = 15.50m,
                SalePrice = 18.00m,
                Stock = 200,
                StockUpper = 500,
                StockLower = 50,
                ProductionDate = new DateTime(2023, 1, 15),
                ExpiryDate = new DateTime(2025, 1, 14),
                Effect = "用于治疗敏感菌所致的呼吸道感染、尿路感染、皮肤软组织感染等。",
                Category = DrugCategory.Western,
            }, autoSave: true);

            await _drugRepository.InsertAsync(new Drug
            {
                DrugName = "布洛芬缓释胶囊",
                DrugType = "解热镇痛",
                FeeName = "西药费",
                DosageForm = "胶囊剂",
                Specification = "300mg*20粒",
                PurchasePrice = 12.00m,
                SalePrice = 15.50m,
                Stock = 350,
                StockUpper = 600,
                StockLower = 100,
                ProductionDate = new DateTime(2023, 5, 20),
                ExpiryDate = new DateTime(2025, 5, 19),
                Effect = "用于缓解轻至中度疼痛如头痛、关节痛、偏头痛、牙痛、肌肉痛、神经痛、痛经。也用于普通感冒或流行性感冒引起的发热。",
                Category = DrugCategory.Western,
            }, autoSave: true);
            
            await _drugRepository.InsertAsync(new Drug
            {
                DrugName = "连花清瘟颗粒",
                DrugType = "清热解毒",
                FeeName = "中成药费",
                DosageForm = "颗粒剂",
                Specification = "6g*10袋",
                PurchasePrice = 25.00m,
                SalePrice = 30.00m,
                Stock = 150,
                StockUpper = 400,
                StockLower = 40,
                ProductionDate = new DateTime(2023, 8, 1),
                ExpiryDate = new DateTime(2025, 7, 31),
                Effect = "清瘟解毒，宣肺泄热。用于治疗流行性感冒属热毒袭肺证，症见发热或高热，恶寒，肌肉酸痛，鼻塞流涕，咳嗽，头痛，咽干咽痛，舌偏红，苔黄或黄腻等。",
                Category = DrugCategory.Chinese,
            }, autoSave: true);

            await _drugRepository.InsertAsync(new Drug
            {
                DrugName = "氢氯噻嗪片",
                DrugType = "利尿降压",
                FeeName = "西药费",
                DosageForm = "片剂",
                Specification = "25mg*100片",
                PurchasePrice = 8.00m,
                SalePrice = 10.00m,
                Stock = 500,
                StockUpper = 1000,
                StockLower = 200,
                ProductionDate = new DateTime(2022, 11, 10),
                ExpiryDate = new DateTime(2024, 11, 9),
                Effect = "用于治疗水肿性疾病、高血压、尿崩症。",
                Category = DrugCategory.Western,
            }, autoSave: true);

            await _drugRepository.InsertAsync(new Drug
            {
                DrugName = "云南白药气雾剂",
                DrugType = "活血化瘀",
                FeeName = "中成药费",
                DosageForm = "气雾剂",
                Specification = "85g+60g",
                PurchasePrice = 35.00m,
                SalePrice = 42.00m,
                Stock = 80,
                StockUpper = 200,
                StockLower = 20,
                ProductionDate = new DateTime(2023, 3, 5),
                ExpiryDate = new DateTime(2025, 3, 4),
                Effect = "活血散瘀，消肿止痛。用于跌打损伤，瘀血肿痛，肌肉酸痛及风湿疼痛。",
                Category = DrugCategory.Chinese,
            }, autoSave: true);
        }
    }
} 