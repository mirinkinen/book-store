using Cataloging.Api;
using Cataloging.IntegrationTests.Fakes;
using Common.Application.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cataloging.IntegrationTests;

public class ApiTestWebApplicationFactory : WebApplicationFactory<Program>
{
    public Action<IServiceCollection>? ConfigureServices { get; set; }

    public IUserService UserService { get; } = new FakeUserService();

    public IHost Host { get; private set; }

    public string ConnectionString { get; private set; }

    public ApiTestWebApplicationFactory()
    {
        ConnectionString = $"Data Source=(localdb)\\BookStoreTest;Initial Catalog={Guid.NewGuid():N};Integrated Security=True";
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        Host = base.CreateHost(builder);
        return Host;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        OverrideAppSettings(builder);

        base.ConfigureWebHost(builder);

        builder.ConfigureServices(services =>
        {
            // Replace IUserService.
            var userServiceDescriptor = services.Single(d => d.ImplementationType == typeof(UserService));
            services.Remove(userServiceDescriptor);
            services.AddScoped<IUserService>(sp => UserService);

            ConfigureServices?.Invoke(services);
        });
    }

    private void OverrideAppSettings(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:CatalogConnectionString", ConnectionString);
    }
}