using Common.API.Auditing;
using Common.Application.Auditing;
using Common.Application.Messages;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.OData.ModelBuilder;
using Oakton;
using Ordering.Application.GetShoppingCart;
using Ordering.Domain;
using Ordering.Infra.Database;
using System.Reflection;
using Wolverine;
using Wolverine.Transports.Tcp;

namespace Ordering;

public static class ServiceConfigurator
{
    internal static void ConfigureServices(WebApplicationBuilder builder, string connectionString)
    {
        // All commands are handled by Wolverine.
        builder.Host.UseWolverine(opts =>
        {
            opts.ServiceName = "Order API";
            opts.ApplicationAssembly = Assembly.GetExecutingAssembly();

            Common.Application.ServiceConfigurator.UseCommonWolverineApplicationSettings(opts);
            Common.Infra.ServiceConfigurator.UseCommonWolverineInfrastructureSettings(opts, connectionString);

            opts.ListenAtPort(5202).UseDurableInbox();
            opts.PublishMessage<Ping>().ToPort(5201).UseDurableOutbox();
        });

        ConfigureApiServices(builder);
        ConfigureApplicationServices(builder);
        ConfigureInfrastructureServices(builder, connectionString);
    }

    private static void ConfigureApiServices(WebApplicationBuilder builder)
    {
        builder.Host.ApplyOaktonExtensions();
        
        // Add services to the container.
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHttpContextAccessor();

        builder.Services.Configure<AuditOptions>(builder.Configuration.GetSection(AuditOptions.Audit));

        AddOData(builder);
    }

    private static void AddOData(WebApplicationBuilder builder)
    {
        var modelBuilder = new ODataConventionModelBuilder();

        var orderEntity = modelBuilder.EntityType<Order>();
        orderEntity.HasKey(book => book.Id);
        orderEntity.Property(book => book.CreatedAt);
        orderEntity.Property(book => book.ModifiedAt);
        orderEntity.Property(book => book.ModifiedBy);
        var orderEntitySet = modelBuilder.EntitySet<Order>("Orders");

        builder.Services.AddControllers().AddOData(
            options => options
                .Count()
                .Expand()
                .Filter()
                .OrderBy()
                .Select()
                .SetMaxTop(20)
                .AddRouteComponents("v1", modelBuilder.GetEdmModel(), services =>
                {
                    // OData seems to have its own container.
                    // Register services here that are used in overridden OData implementations.
                    services.AddSingleton<ODataResourceSerializer, AuditingODataResourceSerializer>();
                    services.AddHttpContextAccessor();
                    services.Configure<AuditOptions>(builder.Configuration.GetSection(AuditOptions.Audit));
                }));
    }
    
    private static void ConfigureApplicationServices(WebApplicationBuilder builder)
    {
        Common.Application.ServiceConfigurator.ConfigureApplicationServices(builder.Services);
    }

    private static void ConfigureInfrastructureServices(WebApplicationBuilder builder, string connectionString)
    {
        Common.Infra.ServiceConfigurator.ConfigureInfrastructureServices<OrderDbContext>(builder.Services,
            connectionString);
    }
}