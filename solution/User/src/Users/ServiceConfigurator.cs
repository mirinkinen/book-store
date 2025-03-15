using Common.API.Auditing;
using Common.Application.Authentication;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Oakton;
using Oakton.Resources;
using System.Reflection;
using Users.API;
using Users.Infra.Database;
using Users.Infra.Database.Setup;

namespace Users;

public static class ServiceConfigurator
{
    internal static void ConfigureServices(this WebApplicationBuilder builder, string connectionString)
    {
        ConfigureApiServices(builder);
        ConfigureApplicationServices(builder);
        ConfigureInfrastructureServices(builder, connectionString);
    }


    private static void ConfigureApiServices(WebApplicationBuilder builder)
    {
        builder.Host.ApplyOaktonExtensions();
        builder.Services.AddAuthorization();
        builder.Services
            .AddGraphQLServer()
            .AddQueryType<Queries>()
            .RegisterDbContextFactory<UserDbContext>()
            .AddProjections()
            .AddFiltering()
            .AddSorting();
        builder.Services.AddScoped<UserDbContext>();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApi();
        builder.Services.AddHttpContextAccessor();

        builder.Services.Configure<AuditOptions>(builder.Configuration.GetSection(AuditOptions.Audit));
    }

    private static void ConfigureApplicationServices(WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        Common.Application.ServiceConfigurator.ConfigureApplicationServices(builder.Services);

        builder.Services.AddSingleton<IUserAccessor, UserAccessor>();
    }

    private static void ConfigureInfrastructureServices(WebApplicationBuilder builder, string connectionString)
    {
        builder.Services.AddPooledDbContextFactory<UserDbContext>(options =>
        {
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
            options.UseSqlServer(connectionString);
        });
    }
}