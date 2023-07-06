namespace Cataloging.IntegrationTests.Wolverine;

public class WolverineTests
{
    [Fact(Skip = "Does not work atm")]
    [Trait("Category", "Wolverine")]
    public void WolverineConfigurationIsValid()
    {
        using var factory = new WolverineValidationFactory();
        // Starts application just to execute the AssertWolverineConfigurationIsValid method.
        using var client = factory.CreateClient();
    }
}