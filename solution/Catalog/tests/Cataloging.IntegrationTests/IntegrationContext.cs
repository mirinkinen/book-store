using Alba;
using Cataloging.IntegrationTests.Fakes;
using Common.Application.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Oakton;
using Wolverine.Runtime;

namespace Cataloging.IntegrationTests;

public class AppFixture : IAsyncLifetime
{
    public IAlbaHost? Host { get; private set; }

    public FakeUserService UserService { get; } = new();

    public async Task InitializeAsync()
    {
        // Sorry folks, but this is absolutely necessary if you 
        // use Oakton for command line processing and want to 
        // use WebApplicationFactory and/or Alba for integration testing
        OaktonEnvironment.AutoStartHost = true;

        // This is bootstrapping the actual application using
        // its implied Program.Main() set up
        Host = await AlbaHost.For<Program>(builder =>
        {
            builder.ConfigureServices(services => { services.AddScoped<IUserService>(_ => UserService); });
        });
    }

    public Task DisposeAsync()
    {
        return Host!.StopAsync();
    }
}

[CollectionDefinition("integration")]
public class IntegrationCollection : ICollectionFixture<AppFixture>
{
}

[Collection("integration")]
public abstract class IntegrationContext : IAsyncLifetime
{
    private readonly AppFixture _fixture;

    protected IntegrationContext(AppFixture fixture)
    {
        _fixture = fixture;
        Runtime = (WolverineRuntime)fixture.Host!.Services.GetRequiredService<IWolverineRuntime>();
    }

    protected WolverineRuntime Runtime { get; }

    protected IAlbaHost Host => _fixture.Host!;

    public Task InitializeAsync()
    {
        // Using Marten, wipe out all data and reset the state
        // back to exactly what we described in InitialAccountData
        return Task.CompletedTask;
    }

    // This is required because of the IAsyncLifetime 
    // interface. Note that I do *not* tear down database
    // state after the test. That's purposeful
    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}