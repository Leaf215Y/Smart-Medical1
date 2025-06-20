using System;
using System.Collections.Generic;
using System.Text;
using Smart_Medical.Localization;
using Volo.Abp.Application.Services;

namespace Smart_Medical;

/* Inherit your application services from this class.
 */
public abstract class Smart_MedicalAppService : ApplicationService
{
    protected Smart_MedicalAppService()
    {
        LocalizationResource = typeof(Smart_MedicalResource);
    }
}
