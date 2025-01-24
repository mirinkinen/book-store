using System.Text.Json;
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

    public TestDatabase TestDatabase { get; } = new();

    public async ValueTask InitializeAsync()
    {
        await TestDatabase.CleanLeftoverDatabasesAsync();
        await TestDatabase.CreateAndSeedDatabase();
        
        // Sorry folks, but this is absolutely necessary if you 
        // use Oakton for command line processing and want to 
        // use WebApplicationFactory and/or Alba for integration testing
        OaktonEnvironment.AutoStartHost = true;

        // This is bootstrapping the actual application using
        // its implied Program.Main() set up
        Host = await AlbaHost.For<Program>(builder =>
        {
            builder.UseSetting("ConnectionStrings:CatalogConnectionString", TestDatabase.ConnectionString);
            
            builder.ConfigureServices(services => { services.AddScoped<IUserService>(_ => UserService); });
        });
    }

    public async ValueTask DisposeAsync()
    {
        await TestDatabase.DisposeAsync();
        await Host!.StopAsync();
    }
}

[CollectionDefinition("Integration")]
public class IntegrationCollection : ICollectionFixture<AppFixture>
{
}

[Collection("Integration")]
public abstract class IntegrationContext : IAsyncLifetime
{
    private readonly AppFixture _app;

    protected IntegrationContext(AppFixture app)
    {
        _app = app;
        Runtime = (WolverineRuntime)app.Host!.Services.GetRequiredService<IWolverineRuntime>();
        SerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public JsonSerializerOptions SerializerOptions { get; }

    protected WolverineRuntime Runtime { get; }

    protected IAlbaHost Host => _app.Host!;

    protected FakeUserService UserService => _app.UserService;

    public ValueTask InitializeAsync()
    {
        // Using Marten, wipe out all data and reset the state
        // back to exactly what we described in InitialAccountData
        return ValueTask.CompletedTask;
    }

    // This is required because of the IAsyncLifetime 
    // interface. Note that I do *not* tear down database
    // state after the test. That's purposeful
    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }
}