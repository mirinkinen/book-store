using Cataloging.Application.Services;
using Cataloging.Domain.Authors;
using Cataloging.Infrastructure.Database;
using Cataloging.Infrastructure.Queries;
using Cataloging.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.SqlServer;

namespace Cataloging.Infrastructure;

public static class ServiceRegistrar
{
    public static void RegisterInfrastructureServices(IServiceCollection services, string connectionString)
    {
        services.AddDbContextWithWolverineIntegration<CatalogDbContext>(dbContextOptions =>
            {
                dbContextOptions.UseSqlServer(connectionString);
            });

        services.AddScoped<IQueryAuthorizer, QueryAuthorizer>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();
    }

    public static void UseWolverine(WolverineOptions opts, string connectionString)
    {
        // Setting up Sql Server-backed message storage
        // This requires a reference to Wolverine.SqlServer
        opts.PersistMessagesWithSqlServer(connectionString);

        // Enrolling all local queues into the
        // durable inbox/outbox processing
        opts.Policies.UseDurableLocalQueues();

        // Add the auto transaction middleware attachment policy
        // If enabled, handlers don't need [AutoApplyTransactions] attribute.
        //opts.Policies.AutoApplyTransactions();
    }
}