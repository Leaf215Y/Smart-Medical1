using Smart_Medical.Samples;
using Xunit;

namespace Smart_Medical.EntityFrameworkCore.Applications;

[Collection(Smart_MedicalTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<Smart_MedicalEntityFrameworkCoreTestModule>
{

}
