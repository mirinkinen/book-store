using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Oakton;

namespace Cataloging.IntegrationTests.Wolverine;

[Trait("Category", "Wolverine")]
public class WolverineTests
{
    public WolverineTests()
    {
        OaktonEnvironment.AutoStartHost = true;
    }

    [Fact]
    public void StartAndStop()
    {
        OaktonEnvironment.AutoStartHost = true;

        using var factory = new WebApplicationFactory<Program>();
        using var httpClient = factory.CreateClient();
    }

    [Fact]
    public void ValidateWolverineConfiguration()
    {
        using var factory = new WolverineValidationFactory();
        using var httpClient = factory.CreateClient();
    }
}