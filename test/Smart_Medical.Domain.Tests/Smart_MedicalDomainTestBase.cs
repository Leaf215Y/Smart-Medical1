using Volo.Abp.Modularity;

namespace Smart_Medical;

/* Inherit from this class for your domain layer tests. */
public abstract class Smart_MedicalDomainTestBase<TStartupModule> : Smart_MedicalTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
