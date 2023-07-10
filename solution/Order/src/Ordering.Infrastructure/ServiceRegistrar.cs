using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Domain.Orders;
using Ordering.Infrastructure.Database;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.SqlServer;

namespace Ordering.Infrastructure;

public static class ServiceRegistrar
{
    private const string _wolverineSchema = "wolverine";

    public static void RegisterInfrastructureServices(IServiceCollection services, string connectionString)
    {
        services.AddDbContextWithWolverineIntegration<OrderingDbContext>(dbContextOptions =>
        {
            dbContextOptions.UseSqlServer(connectionString);
        }, _wolverineSchema);

        services.AddScoped<IOrderRepository, OrderRepository>();
    }

    public static void UseWolverine(WolverineOptions opts, string connectionString)
    {
        // Setting up Sql Server-backed message storage
        // This requires a reference to Wolverine.SqlServer
        opts.PersistMessagesWithSqlServer(connectionString, _wolverineSchema);

        // Enrolling all local queues into the
        // durable inbox/outbox processing
        opts.Policies.UseDurableLocalQueues();

        // Add the auto transaction middleware attachment policy
        // If enabled, handlers don't need [AutoApplyTransactions] attribute.
        //opts.Policies.AutoApplyTransactions();
    }
}