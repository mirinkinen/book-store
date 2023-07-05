using Common.Api.Auditing;
using Oakton;
using System.Diagnostics.CodeAnalysis;

namespace Cataloging.Api;

[SuppressMessage("Design", "CA1052:Static holder types should be Static or NotInheritable", Justification = "Must not be static for API tests.")]
public class Program
{
    public static async Task Main(string[] args)
    {
        IEnumerable<string> strings = Enumerable.Empty<string>();

        var builder = WebApplication.CreateBuilder(args);

        var connectionString = builder.Configuration.GetConnectionString("CatalogConnectionString");
        if (builder.Environment.IsEnvironment("Testing"))
        {
            var dbName = $"BookStoreTest_{Guid.NewGuid():N}";
            connectionString = $"Data Source=(localdb)\\BookStore;Initial Catalog={dbName};Integrated Security=True";
        }

        ArgumentNullException.ThrowIfNull(connectionString);

        Api.ServiceRegistrar.RegisterApiServices(builder);
        Application.ServiceRegistrar.RegisterApplicationServices(builder.Services);
        Infrastructure.ServiceRegistrar.RegisterInfrastructureServices(builder.Services, connectionString);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseAuditLogging();
        app.MapControllers();

        await app.RunOaktonCommands(args);
        await app.RunAsync();
    }
}