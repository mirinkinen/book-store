using Alba;
using Common.Application.Auditing;
using Microsoft.Extensions.DependencyInjection;
using Oakton;
using Wolverine.Runtime;
using Wolverine.Tracking;

namespace Cataloging.IntegrationTests;

public class AppFixture : IAsyncLifetime
{
    public IAlbaHost? Host { get; private set; }

    public AuditContext AuditContext { get; } = new();

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
            builder.ConfigureServices(services => services.AddScoped<AuditContext>(sp => AuditContext));
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

    public WolverineRuntime Runtime { get; }

    public IAlbaHost Host => _fixture.Host!;

    Task IAsyncLifetime.InitializeAsync()
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

    public Task<IScenarioResult> Scenario(Action<Scenario> configure)
    {
        return Host.Scenario(configure);
    }
    
    // This method allows us to make HTTP calls into our system
    // in memory with Alba, but do so within Wolverine's test support
    // for message tracking to both record outgoing messages and to ensure
    // that any cascaded work spawned by the initial command is completed
    // before passing control back to the calling test
    protected async Task<(ITrackedSession, IScenarioResult)> TrackedHttpCall(Action<Scenario> configuration)
    {
        IScenarioResult? result = null;

        // The outer part is tying into Wolverine's test support
        // to "wait" for all detected message activity to complete
        var tracked = await Host.ExecuteAndWaitAsync(async () =>
        {
            // The inner part here is actually making an HTTP request
            // to the system under test with Alba
            result = await Host.Scenario(configuration);
        });

        return (tracked!, result!);
    }
}