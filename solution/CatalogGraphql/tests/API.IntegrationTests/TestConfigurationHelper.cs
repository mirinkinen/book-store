using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace API.IntegrationTests;

public static class TestConfigurationHelper
{
    public static IConfiguration CreateTestConfiguration()
    {
        var defaultSettings = new Dictionary<string, string?>
        {
            { "Environment", "Test" },
            { "APPLICATIONINSIGHTS_CONNECTION_STRING", "" }, // Disable Application Insights
            { "ConnectionStrings:DefaultConnection", "Server=(localdb)\\mssqllocaldb;Database=TestDb;Trusted_Connection=true;" }
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(defaultSettings)
            .Build();
    }

    public static IServiceCollection ConfigureTestServices(IConfiguration configuration)
    {
        var services = new ServiceCollection();

        services.AddLogging(c => { c.AddDebug().SetMinimumLevel(LogLevel.Warning); });

        // Add mock TelemetryClient for tests
        services.AddSingleton<TelemetryClient>();

        // Register your application services
        services.RegisterServices(configuration);

        return services;
    }
}