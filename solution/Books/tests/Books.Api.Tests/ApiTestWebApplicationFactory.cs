using Books.Infrastructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data.Common;

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
            ReplaceDatabaseWithSqlite(services);
        });
    }

    private static void ReplaceDatabaseWithSqlite(IServiceCollection services)
    {
        var dbContextDescriptor = services.Single(d => d.ServiceType == typeof(DbContextOptions<BooksDbContext>));
        services.Remove(dbContextDescriptor);

        services.AddSingleton<DbConnection>(container =>
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            return connection;
        });

        services.AddDbContext<BooksDbContext>((container, options) =>
        {
            var connection = container.GetRequiredService<DbConnection>();
            options.UseSqlite(connection);
        });
    }
}