using Smart_Medical.Samples;
using Xunit;

namespace Smart_Medical.EntityFrameworkCore.Domains;

[Collection(Smart_MedicalTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<Smart_MedicalEntityFrameworkCoreTestModule>
{

}
