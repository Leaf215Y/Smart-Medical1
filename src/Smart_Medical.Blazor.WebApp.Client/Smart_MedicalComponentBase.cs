using Smart_Medical.Localization;
using Volo.Abp.AspNetCore.Components;

namespace Smart_Medical.Blazor.WebApp.Client;

public abstract class Smart_MedicalComponentBase : AbpComponentBase
{
    protected Smart_MedicalComponentBase()
    {
        LocalizationResource = typeof(Smart_MedicalResource);
    }
}
