using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace Smart_Medical;

[DependsOn(
    typeof(Smart_MedicalDomainModule),
    typeof(Smart_MedicalApplicationContractsModule),
    typeof(AbpSettingManagementApplicationModule)

    )]
public class Smart_MedicalApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<Smart_MedicalApplicationModule>();
        });

    }
}
