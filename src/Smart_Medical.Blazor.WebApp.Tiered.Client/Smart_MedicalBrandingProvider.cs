using Microsoft.Extensions.Localization;
using Smart_Medical.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Smart_Medical.Blazor.WebApp.Tiered.Client;

[Dependency(ReplaceServices = true)]
public class Smart_MedicalBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<Smart_MedicalResource> _localizer;

    public Smart_MedicalBrandingProvider(IStringLocalizer<Smart_MedicalResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
