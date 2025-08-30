using Cataloging.API.Auditing;
using Cataloging.Application;
using Cataloging.Application.Auditing;
using Cataloging.Application.Middleware;
using Cataloging.Domain;
using Cataloging.Infra;
using Cataloging.Infra.Database;
using Common;
using Common.Application;
using Common.Application.Authentication;
using Common.Application.Messages;
using Common.Domain;
using Common.Infra;
using FluentValidation;
using FluentValidation.AspNetCore;
using JasperFx;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Trace;
using System.Reflection;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.SqlServer;
using Wolverine.Transports.Tcp;

namespace Cataloging;

public static class ServiceConfigurator
{
    private const string _wolverineSchema = "wolverine";
    
    internal static void ConfigureServices(this WebApplicationBuilder builder, string connectionString)
    {
        ConfigureApiServices(builder, connectionString);
        ConfigureApplicationServices(builder);
        ConfigureDomainServices(builder);
        ConfigureInfrastructureServices(builder, connectionString);
    }

    private static void ConfigureDomainServices(WebApplicationBuilder builder)
    {
        var authorizerInterfaceType = typeof(IQueryAuthorizer<>);

        builder.Services.Scan(scanner => scanner.FromAssemblyOf<AuthorQueryAuthorizer>()
            .AddClasses(classes => classes.AssignableTo(authorizerInterfaceType))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        var authorizerRepositoryInterfaceType = typeof(IQueryAuthorizerRepository<>);

        builder.Services.Scan(scanner => scanner.FromAssemblyOf<AuthorQueryAuthorizer>()
            .AddClasses(classes => classes.AssignableTo(authorizerRepositoryInterfaceType))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
    }

    private static void ConfigureApiServices(WebApplicationBuilder builder, string connectionString)
    {
        builder.Host.ApplyJasperFxExtensions();

        // All commands are handled by Wolverine.
        builder.Host.UseWolverine(opts =>
        {
            // API settings.
            opts.Services.AddOpenTelemetry()
                .WithTracing(trace => trace
                    .AddSource("*")
                    .AddAspNetCoreInstrumentation()
                    .AddConsoleExporter());

            if (builder.Environment.IsDevelopment())
            {
                opts.Durability.Mode = DurabilityMode.Solo;
            }

            // Enable to preview generated code upon first call.
            //opts.CodeGeneration.TypeLoadMode = JasperFx.CodeGeneration.TypeLoadMode.Auto;

            builder.Services.AddFluentValidationAutoValidation(config => { config.DisableDataAnnotationsValidation = true; });

            // Disable default member name manipulation.
            ValidatorOptions.Global.DisplayNameResolver = (type, memberInfo, expression) => memberInfo?.Name ?? "";

            // Application settings.
            opts.Policies.LogMessageStarting(LogLevel.Debug);
            opts.Discovery.IncludeAssembly(Assembly.GetExecutingAssembly());
            opts.ServiceName = "Catalog API";
            opts.ApplicationAssembly = Assembly.GetExecutingAssembly();
            opts.Policies.ForMessagesOfType<IAuthorCommand>().AddMiddleware(typeof(LoadAuthorMiddleware));

            // Infrastructure settings.
            // Setting up Sql Server-backed message storage
            // This requires a reference to Wolverine.SqlServer
            opts.PersistMessagesWithSqlServer(connectionString, _wolverineSchema);

            // Adds the usage of DbContextOutbox.
            opts.UseEntityFrameworkCoreTransactions();
        
            // Use durable inbox and outbox.
            opts.Policies.UseDurableLocalQueues();
            opts.Policies.UseDurableInboxOnAllListeners();
            opts.Policies.UseDurableOutboxOnAllSendingEndpoints();

            // Add the auto transaction middleware attachment policy
            // If enabled, handlers don't need [AutoApplyTransactions] attribute.
            // opts.Policies.AutoApplyTransactions();
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
        
        // User
        builder.Services.AddScoped<IUserAccessor, UserAccessor>();

        // Audit.
        builder.Services.AddScoped<AuditContext>();
        
    }

    private static void ConfigureInfrastructureServices(WebApplicationBuilder builder, string connectionString)
    {
        builder.Services.AddDbContextWithWolverineIntegration<CatalogDbContext>(dbContextOptions =>
        {
            dbContextOptions.UseSqlServer(connectionString);
        }, _wolverineSchema);

        builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
    }
}