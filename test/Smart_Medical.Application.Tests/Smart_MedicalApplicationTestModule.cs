using Volo.Abp.Modularity;

namespace Smart_Medical;

[DependsOn(
    typeof(Smart_MedicalApplicationModule),
    typeof(Smart_MedicalDomainTestModule)
)]
public class Smart_MedicalApplicationTestModule : AbpModule
{

}
