using Common.Application.Messages;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Infrastructure.Database;
using Wolverine;
using Wolverine.Transports.Tcp;

namespace Ordering.Infrastructure;

public static class ServiceRegistrar
{
    public static void RegisterInfrastructureServices(IServiceCollection services, string connectionString)
    {
        Common.Infrastructure.ServiceRegistrar.RegisterInfrastructureServices<OrderDbContext>(services,
            connectionString);
    }

    public static void UseWolverine(WolverineOptions opts, string connectionString)
    {
        opts.ListenAtPort(5202).UseDurableInbox();
        opts.PublishMessage<Ping>().ToPort(5201).UseDurableOutbox();
        
        Common.Infrastructure.ServiceRegistrar.UseWolverine(opts, connectionString);
    }
}