using Volo.Abp.Settings;

namespace Smart_Medical.Settings;

public class Smart_MedicalSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(Smart_MedicalSettings.MySetting1));
    }
}
