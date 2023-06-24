using Books.Infrastructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Books.Api.Tests;

public class ApiTestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.UseEnvironment("Development");
        base.ConfigureWebHost(builder);

        builder.ConfigureServices(services =>
        {
            // Remove the real database connection
            var dbContextDescriptor = services.Single(d => d.ServiceType == typeof(DbContextOptions<BooksDbContext>));
            services.Remove(dbContextDescriptor);

            services.AddDbContext<BooksDbContext>(dbContextOptions =>
            {
                dbContextOptions.UseInMemoryDatabase("BookStoreTest");
            });
        });
    }

    public Task InitializeDatabase()
    {
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BooksDbContext>();

        return DataSeeder.SeedData(dbContext);
    }
}