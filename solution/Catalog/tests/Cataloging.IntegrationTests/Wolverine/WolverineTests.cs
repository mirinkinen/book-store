using Alba;
using Oakton;
using Wolverine;

namespace Cataloging.IntegrationTests.Wolverine;

[Trait("Category", "Wolverine")]
public class WolverineTests
{
    public WolverineTests()
    {
        OaktonEnvironment.AutoStartHost = true;
    }

    [Fact]
    public async Task WolverineConfigurationIsValid()
    {
        var host = await AlbaHost.For<Program>();
        host.AssertWolverineConfigurationIsValid();
    }
}