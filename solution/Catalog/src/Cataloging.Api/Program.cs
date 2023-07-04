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

        Api.ServiceRegistrar.RegisterApiServices(builder);
        Application.ServiceRegistrar.RegisterApplicationServices(builder.Services);
        Infrastructure.ServiceRegistrar.RegisterInfrastructureServices(builder.Services);

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