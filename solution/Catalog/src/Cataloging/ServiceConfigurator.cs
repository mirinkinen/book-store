using Cataloging.API;
using Cataloging.Application;
using Cataloging.Application.GetBooks;
using Cataloging.Application.Middleware;
using Cataloging.Domain;
using Cataloging.Infra;
using Cataloging.Infra.Database;
using Cataloging.Infra.Database.Setup;
using Common;
using Common.API.Auditing;
using Common.Application;
using Common.Application.Messages;
using Common.Infra;
using FluentValidation;
using Oakton;
using Oakton.Resources;
using Wolverine;
using Wolverine.Transports.Tcp;

namespace Cataloging;

public static class ServiceConfigurator
{
    internal static void ConfigureServices(WebApplicationBuilder builder, string connectionString)
    {
        builder.Host.ApplyOaktonExtensions();
        builder.Services.AddScoped<IStatefulResource, DatabaseInitializer>();

        // All commands are handled by Wolverine.
        builder.Host.UseWolverine(opts =>
        {
            // API settings.
            opts.UseCommonApiSettings(builder);

            // Application settings.
            opts.UseCommonApplicationSettings();
            opts.ServiceName = "Catalog API";
            opts.ApplicationAssembly = typeof(GetBooksHandler).Assembly;
            opts.Policies.ForMessagesOfType<IAuthorCommand>().AddMiddleware(typeof(LoadAuthorMiddleware));

            // Infrastructure settings.
            opts.UseCommonInfrastructureSettings(connectionString);
            opts.ListenAtPort(5201).UseDurableInbox();
            opts.PublishMessage<Pong>().ToPort(5202).UseDurableOutbox();
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
        builder.Services.AddSwaggerGen();
        builder.Services.AddHttpContextAccessor();

        builder.Services.Configure<AuditOptions>(builder.Configuration.GetSection(AuditOptions.Audit));

        builder.AddOData();
        builder.AddGraphQLConfiguration();
    }

    private static void ConfigureApplicationServices(WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssemblyContaining<PutAuthorDtoV1Validator>();
        Common.Application.ServiceConfigurator.RegisterApplicationServices(builder.Services);
    }

    private static void ConfigureInfrastructureServices(WebApplicationBuilder builder, string connectionString)
    {
        Common.Infra.ServiceConfigurator.RegisterInfrastructureServices<CatalogDbContext>(builder.Services,
            connectionString);

        builder.Services.AddScoped<IQueryAuthorizer, QueryAuthorizer>();
        builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
    }
}