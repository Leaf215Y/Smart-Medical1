using Volo.Abp.Modularity;

namespace Smart_Medical;

public abstract class Smart_MedicalApplicationTestBase<TStartupModule> : Smart_MedicalTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
