using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace API.IntegrationTests;

public static class TestConfigurationHelper
{
    public static IConfiguration CreateTestConfiguration(Dictionary<string, string?>? additionalSettings = null)
    {
        var defaultSettings = new Dictionary<string, string?>
        {
            { "Environment", "Test" },
            { "ApplicationInsights:ConnectionString", "" }, // Disable Application Insights
            { "ConnectionStrings:DefaultConnection", "Server=(localdb)\\mssqllocaldb;Database=TestDb;Trusted_Connection=true;" }
        };

        if (additionalSettings != null)
        {
            foreach (var setting in additionalSettings)
            {
                defaultSettings[setting.Key] = setting.Value;
            }
        }

        return new ConfigurationBuilder()
            .AddInMemoryCollection(defaultSettings)
            .Build();
    }

    public static IServiceCollection ConfigureTestServices(IConfiguration configuration)
    {
        var services = new ServiceCollection();

        // Add hosting environment services that Application Insights might need
        services.AddSingleton<IWebHostEnvironment>(provider => new MockWebHostEnvironment());

        #pragma warning disable CS0618 // Type or member is obsolete
        services.AddSingleton<IHostingEnvironment>(provider => 
            new MockHostingEnvironment(provider.GetRequiredService<IWebHostEnvironment>()));
        #pragma warning restore CS0618

        // Register your application services
        services.RegisterServices(configuration);

        return services;
    }

    public static ServiceProvider BuildTestServiceProvider(Dictionary<string, string?>? additionalSettings = null)
    {
        var configuration = CreateTestConfiguration(additionalSettings);
        var services = ConfigureTestServices(configuration);
        return services.BuildServiceProvider();
    }
}

public class MockWebHostEnvironment : IWebHostEnvironment
{
    public string EnvironmentName { get; set; } = "Test";
    public string ApplicationName { get; set; } = "TestApp";
    public string WebRootPath { get; set; } = "";
    public IFileProvider WebRootFileProvider { get; set; } = new NullFileProvider();
    public string ContentRootPath { get; set; } = Directory.GetCurrentDirectory();
    public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
}

#pragma warning disable CS0618 // Type or member is obsolete
public class MockHostingEnvironment : IHostingEnvironment
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public MockHostingEnvironment(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public string EnvironmentName 
    { 
        get => _webHostEnvironment.EnvironmentName; 
        set => _webHostEnvironment.EnvironmentName = value; 
    }

    public string ApplicationName 
    { 
        get => _webHostEnvironment.ApplicationName; 
        set => _webHostEnvironment.ApplicationName = value; 
    }

    public string WebRootPath 
    { 
        get => _webHostEnvironment.WebRootPath; 
        set => _webHostEnvironment.WebRootPath = value; 
    }

    public IFileProvider WebRootFileProvider 
    { 
        get => _webHostEnvironment.WebRootFileProvider; 
        set => _webHostEnvironment.WebRootFileProvider = value; 
    }

    public string ContentRootPath 
    { 
        get => _webHostEnvironment.ContentRootPath; 
        set => _webHostEnvironment.ContentRootPath = value; 
    }

    public IFileProvider ContentRootFileProvider 
    { 
        get => _webHostEnvironment.ContentRootFileProvider; 
        set => _webHostEnvironment.ContentRootFileProvider = value; 
    }
}
#pragma warning restore CS0618 // Type or member is obsolete
