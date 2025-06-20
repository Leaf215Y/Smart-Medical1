using Smart_Medical.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Smart_Medical.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class Smart_MedicalController : AbpControllerBase
{
    protected Smart_MedicalController()
    {
        LocalizationResource = typeof(Smart_MedicalResource);
    }
}
