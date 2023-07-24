using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.SqlServer;

namespace Common.Infrastructure;

public static class ServiceRegistrar
{
    private const string _wolverineSchema = "wolverine";

    public static void RegisterInfrastructureServices<TDbContext>(IServiceCollection services, string connectionString) where 
        TDbContext : DbContext
    {
        services.AddDbContextWithWolverineIntegration<TDbContext>(dbContextOptions =>
        {
            dbContextOptions.UseSqlServer(connectionString);
        }, _wolverineSchema);

    }

    public static void UseWolverine(WolverineOptions opts, string connectionString)
    {
        opts.Policies.UseDurableInboxOnAllListeners();
        opts.Policies.UseDurableOutboxOnAllSendingEndpoints();
        
        // Setting up Sql Server-backed message storage
        // This requires a reference to Wolverine.SqlServer
        opts.PersistMessagesWithSqlServer(connectionString, _wolverineSchema);

        // Adds the usage of DbContextOutbox.
        opts.UseEntityFrameworkCoreTransactions();
        
        // Enrolling all local queues into the
        // durable inbox/outbox processing
        opts.Policies.UseDurableLocalQueues();

        // Add the auto transaction middleware attachment policy
        // If enabled, handlers don't need [AutoApplyTransactions] attribute.
        // opts.Policies.AutoApplyTransactions();
    }
}