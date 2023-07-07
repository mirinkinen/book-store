using Microsoft.AspNetCore.Mvc.Testing;
using Oakton;

namespace Cataloging.IntegrationTests.Wolverine;

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

        var factory = new WebApplicationFactory<Program>();
        using var httpClient = factory.CreateClient();
    }

    [Fact]
    public void ValidateWolverineConfiguration()
    {
        var factory = new WolverineValidationFactory();
        using var httpClient = factory.CreateClient();
    }
}