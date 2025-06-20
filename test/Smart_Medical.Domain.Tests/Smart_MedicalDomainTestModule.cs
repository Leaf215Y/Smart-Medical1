using Volo.Abp.Modularity;

namespace Smart_Medical;

[DependsOn(
    typeof(Smart_MedicalDomainModule),
    typeof(Smart_MedicalTestBaseModule)
)]
public class Smart_MedicalDomainTestModule : AbpModule
{

}
