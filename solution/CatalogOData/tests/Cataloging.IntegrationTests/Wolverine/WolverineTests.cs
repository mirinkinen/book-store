using Alba;
using JasperFx.CommandLine;
using JasperFx;
using Wolverine;

namespace Cataloging.IntegrationTests.Wolverine;

[Trait("Category", "Wolverine")]
public class WolverineTests
{
    public WolverineTests()
    {
        JasperFxEnvironment.AutoStartHost = true;
    }

    [Fact]
    public async Task WolverineConfigurationIsValid()
    {
        var host = await AlbaHost.For<Program>();
        host.AssertWolverineConfigurationIsValid();
    }
}