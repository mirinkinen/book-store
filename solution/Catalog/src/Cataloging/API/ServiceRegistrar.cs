using Cataloging.Application;
using Cataloging.Infra.Database;
using Cataloging.Infra.Database.Setup;
using Cataloging.Infra.Queries;
using Cataloging.Requests.Authors.Application;
using Cataloging.Requests.Authors.Application.Middleware;
using Cataloging.Requests.Authors.Application.UpdateAuthor;
using Cataloging.Requests.Authors.Domain;
using Cataloging.Requests.Authors.Infra;
using Cataloging.Requests.Books.Application.GetBooks;
using Cataloging.Schema;
using Cataloging.Schema.Types;
using Common.API;
using Common.API.Auditing;
using Common.Application;
using Common.Application.Messages;
using Common.Infra;
using FluentValidation;
using FluentValidation.AspNetCore;
using GraphQL;
using Microsoft.AspNetCore.Components.Forms;
using Oakton;
using Oakton.Resources;
using Wolverine;
using Wolverine.Transports.Tcp;

namespace Cataloging.API;

public static class ServiceRegistrar
{
    internal static void RegisterServices(WebApplicationBuilder builder, string connectionString)
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

        builder.Services.AddGraphQL(o =>
        {
            o.AddSchema<CatalogSchema>();
            o.AddDataLoader();
            o.UseTelemetry();
            o.AddSystemTextJson();
        });
        builder.Services.AddScoped<CatalogQuery>();
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
    }

    private static void ConfigureApplicationServices(WebApplicationBuilder builder)
    {
        builder.Services.AddFluentValidationAutoValidation(config =>
        {
            config.DisableDataAnnotationsValidation = true;
        });
        
        builder.Services.AddValidatorsFromAssemblyContaining<AuthorPutValidator>();
        Common.Application.ServiceRegistrar.RegisterApplicationServices(builder.Services);
    }

    private static void ConfigureInfrastructureServices(WebApplicationBuilder builder, string connectionString)
    {
        Common.Infra.ServiceRegistrar.RegisterInfrastructureServices<CatalogDbContext>(builder.Services,
            connectionString);

        builder.Services.AddScoped<IQueryAuthorizer, QueryAuthorizer>();
        builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
    }
}