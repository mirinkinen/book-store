using Cataloging.Application;
using Cataloging.Application.Middleware;
using Cataloging.Domain;
using Cataloging.Infra;
using Cataloging.Infra.Database;
using Common;
using Common.API.Auditing;
using Common.Application;
using Common.Application.Messages;
using Common.Infra;
using FluentValidation;
using JasperFx;
using System.Reflection;
using Wolverine;
using Wolverine.Transports.Tcp;

namespace Cataloging;

public static class ServiceConfigurator
{
    internal static void ConfigureServices(this WebApplicationBuilder builder, string connectionString)
    {
        ConfigureApiServices(builder, connectionString);
        ConfigureApplicationServices(builder);
        ConfigureDomainServices(builder);
        ConfigureInfrastructureServices(builder, connectionString);
    }

    private static void ConfigureDomainServices(WebApplicationBuilder builder)
    {
        builder.ConfigureCommonDomainServices<AuthorQueryAuthorizer>();
    }

    private static void ConfigureApiServices(WebApplicationBuilder builder, string connectionString)
    {
        builder.Host.ApplyJasperFxExtensions();

        // All commands are handled by Wolverine.
        builder.Host.UseWolverine(opts =>
        {
            // API settings.
            opts.UseCommonWolverineApiSettings(builder);

            // Application settings.
            opts.UseCommonWolverineApplicationSettings();
            opts.ServiceName = "Catalog API";
            opts.ApplicationAssembly = Assembly.GetExecutingAssembly();
            opts.Policies.ForMessagesOfType<IAuthorCommand>().AddMiddleware(typeof(LoadAuthorMiddleware));

            // Infrastructure settings.
            opts.UseCommonWolverineInfrastructureSettings(connectionString);
            opts.ListenAtPort(5201).UseDurableInbox();
            opts.PublishMessage<Pong>().ToPort(5202).UseDurableOutbox();
        });

        // Add services to the container.
        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHttpContextAccessor();

        builder.Services.Configure<AuditOptions>(builder.Configuration.GetSection(AuditOptions.Audit));

        builder.AddOData();
    }

    private static void ConfigureApplicationServices(WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        Common.Application.ServiceConfigurator.ConfigureApplicationServices(builder.Services);
    }

    private static void ConfigureInfrastructureServices(WebApplicationBuilder builder, string connectionString)
    {
        Common.Infra.ServiceConfigurator.ConfigureInfrastructureServices<CatalogDbContext>(builder.Services,
            connectionString);

        builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
    }
}