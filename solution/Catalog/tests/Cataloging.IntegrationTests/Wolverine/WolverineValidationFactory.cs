using Cataloging.Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Wolverine;

namespace Cataloging.IntegrationTests;

public class WolverineValidationFactory : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);
        host.AssertWolverineConfigurationIsValid();

        return host;
    }
}