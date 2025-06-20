using Smart_Medical.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Smart_Medical.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(Smart_MedicalEntityFrameworkCoreModule),
    typeof(Smart_MedicalApplicationContractsModule)
    )]
public class Smart_MedicalDbMigratorModule : AbpModule
{
}
