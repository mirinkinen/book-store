using Cataloging.IntegrationTests.Fakes;
using Common.Application.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Cataloging.IntegrationTests;

internal class IntegrationWebApplicationFactory : WebApplicationFactory<Program>
{
    public Action<IServiceCollection>? ConfigureServices { get; set; }

    public IUserService UserService { get; }

    public TestDatabase TestDatabase { get; }

    public IntegrationWebApplicationFactory()
    {
        UserService = new FakeUserService();
        TestDatabase = new TestDatabase();
    }

    public override async ValueTask DisposeAsync()
    {
        // Dispose the host before killing the database, or message draining against the database will fail.
        await base.DisposeAsync();
        await TestDatabase.DisposeAsync();
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
        builder.UseSetting("ConnectionStrings:CatalogConnectionString", TestDatabase.ConnectionString);
    }
}