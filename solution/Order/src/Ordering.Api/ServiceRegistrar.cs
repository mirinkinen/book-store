using Common.Api.Auditing;
using Common.Application.Auditing;
using Common.Application.Messages;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.OData.ModelBuilder;
using Oakton;
using Ordering.Application.Requests.GetShoppingCart;
using Ordering.Domain.Orders;
using Ordering.Infrastructure.Database;
using Wolverine;
using Wolverine.Transports.Tcp;

namespace Ordering.Api;

public static class ServiceRegistrar
{
    internal static void RegisterServices(WebApplicationBuilder builder, string connectionString)
    {
        builder.Host.ApplyOaktonExtensions();

        // All commands are handled by Wolverine.
        builder.Host.UseWolverine(opts =>
        {
            opts.ServiceName = "Order API";
            opts.Discovery.IncludeAssembly(typeof(GetShoppingCartQuery).Assembly);
            opts.Discovery.IncludeAssembly(typeof(AuditLogEventHandler).Assembly);

            opts.Policies.LogMessageStarting(LogLevel.Debug);

            Common.Application.ServiceRegistrar.RegisterApplicationServices(builder.Services);
            Common.Application.ServiceRegistrar.UseWolferine(opts);
            Common.Infrastructure.ServiceRegistrar.UseWolverine(opts, connectionString);

            opts.ListenAtPort(5202).UseDurableInbox();
            opts.PublishMessage<Ping>().ToPort(5201).UseDurableOutbox();
            
            //opts.CodeGeneration.TypeLoadMode = JasperFx.CodeGeneration.TypeLoadMode.Auto;
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
        Common.Application.ServiceRegistrar.RegisterApplicationServices(builder.Services);
    }

    private static void ConfigureInfrastructureServices(WebApplicationBuilder builder, string connectionString)
    {
        Common.Infrastructure.ServiceRegistrar.RegisterInfrastructureServices<OrderDbContext>(builder.Services,
            connectionString);
    }
}