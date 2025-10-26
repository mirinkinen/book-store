using Application.OrderCommands.CreateOrder;
using Application.OrderItemQueries;
using Application.OrderQueries;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using Common.Domain;
using Domain.OrderItems;
using Domain.Orders;
using HotChocolate.Diagnostics;
using HotChocolate.Execution;
using Infra.Database;
using Infra.Repositories;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace API;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configures services for the application.
    /// </summary>
    /// <remarks>This method is also called in integration tests. Place everything here that should be used both in production and
    /// integration tests.</remarks>
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureGraphql();
        services.ConfigureInfraServices(configuration);

        services.AddMediatR(conf => { conf.RegisterServicesFromAssemblyContaining<CreateOrderHandler>(); });

        return services;
    }

    public static void ConfigureLogging(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Application Insights telemetry
        services.AddApplicationInsightsTelemetry(configuration);

        // Configure telemetry initializer
        services.AddSingleton<ITelemetryInitializer, GraphQLTelemetryInitializer>();

        var connectionString = configuration.GetValue<string>("APPLICATIONINSIGHTS_CONNECTION_STRING");

        services.AddOpenTelemetry()
            .WithTracing(b =>
            {
                b.AddHttpClientInstrumentation();
                b.AddAspNetCoreInstrumentation();
                b.AddHotChocolateInstrumentation();
            }).WithMetrics(b =>
            {
                b.AddHttpClientInstrumentation();
                b.AddAspNetCoreInstrumentation();
            }).UseAzureMonitor(c =>
            {
                c.ConnectionString = connectionString;
                c.EnableLiveMetrics = true;
            });
    }

    private static void ConfigureGraphql(this IServiceCollection services)
    {
        services.AddGraphQLServer()
            // Types
            .AddTypes()
            // Conventions
            .AddQueryConventions()
            .AddMutationConventions(new MutationConventionOptions
            {
                ApplyToAllMutations = true,
                InputArgumentName = "command",
                InputTypeNamePattern = "{MutationName}Command",
                PayloadTypeNamePattern = "{MutationName}Dto",
                PayloadErrorsFieldName = "errors",
                PayloadErrorTypeNamePattern = "{MutationName}Error"
            })
            .AddErrorInterfaceType<IUserError>()
            // Error handling and logging
            .AddInstrumentation(options => { options.Scopes = ActivityScopes.All; })
            .AddErrorFilter<GraphQLErrorFilter>()
            // Query capabilities
            .AddProjections()
            .AddFiltering()
            .AddSorting()
            .AddPagingArguments()
            .ModifyPagingOptions(options =>
            {
                options.MaxPageSize = 10;
                options.DefaultPageSize = 10;
                options.IncludeTotalCount = true;
            })
            //.AddMaxExecutionDepthRule(4)
            .ModifyCostOptions(options =>
            {
                options.MaxTypeCost = 5000;
                options.MaxFieldCost = 5000;
            })
            .AddGlobalObjectIdentification()
            // Data store
            .RegisterDbContextFactory<OrdersDbContext>()
            .AddInMemorySubscriptions()
            // Performance
            .InitializeOnStartup(
                warmup: async (executor, cancellationToken) => { await executor.ExecuteAsync("{ __typename }", cancellationToken); });
    }

    private static void ConfigureInfraServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Queryable - Hot chocolate will dispose of the DbContext.
        services.AddScoped<IOrderReadRepository, OrderReadRepository>();
        services.AddScoped<IOrderItemReadRepository, OrderItemReadRepository>();

        // Command
        services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();
        services.AddScoped<IOrderItemWriteRepository, OrderItemWriteRepository>();

        services.AddDbContextPool<OrdersDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
        });
        services.AddPooledDbContextFactory<OrdersDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
        });
    }
}