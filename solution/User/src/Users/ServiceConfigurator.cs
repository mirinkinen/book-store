using Common;
using Common.API.Auditing;
using Common.Application;
using Common.Infra;
using FluentValidation;
using Oakton;
using Oakton.Resources;
using System;
using System.Reflection;
using Users.Infra.Database;
using Users.Infra.Database.Setup;
using Wolverine;

namespace Users;

public class ServiceConfigurator
{
 internal static void ConfigureServices(WebApplicationBuilder builder, string connectionString)
    {
        builder.Host.ApplyOaktonExtensions();
        builder.Services.AddScoped<IStatefulResource, DatabaseInitializer>();

        // All commands are handled by Wolverine.
        builder.Host.UseWolverine(opts =>
        {
            // API settings.
            opts.UseCommonWolverineApiSettings(builder);

            // Application settings.
            opts.UseCommonWolverineApplicationSettings();
            opts.ServiceName = "User API";

            // Infrastructure settings.
            opts.UseCommonWolverineInfrastructureSettings(connectionString);
        });

        ConfigureApiServices(builder);
        ConfigureApplicationServices(builder);
        ConfigureInfrastructureServices(builder, connectionString);
    }

    private static void ConfigureApiServices(WebApplicationBuilder builder)
    {
        // Add services to the container.
        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApi();
        builder.Services.AddHttpContextAccessor();

        builder.Services.Configure<AuditOptions>(builder.Configuration.GetSection(AuditOptions.Audit));
    }

    private static void ConfigureApplicationServices(WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        Common.Application.ServiceConfigurator.ConfigureApplicationServices(builder.Services);
    }

    private static void ConfigureInfrastructureServices(WebApplicationBuilder builder, string connectionString)
    {
        Common.Infra.ServiceConfigurator.ConfigureInfrastructureServices<UserDbContext>(builder.Services,
            connectionString);
    }
}
