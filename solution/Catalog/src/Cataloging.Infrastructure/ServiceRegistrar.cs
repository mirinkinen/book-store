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
    private const string _connectionString = "Data Source=(localdb)\\BookStore;Initial Catalog=BookStore;Integrated Security=True";
    private const string _wolverineDbSchema = "wolverine";

    public static void RegisterInfrastructureServices(IServiceCollection services)
    {
        services.AddDbContextWithWolverineIntegration<CatalogDbContext>(
            dbContextOptions =>
            {
    #pragma warning disable CS8604 // Possible null reference argument.
                dbContextOptions.UseSqlServer(_connectionString);            

    #pragma warning restore CS8604 // Possible null reference argument.
            },
            _wolverineDbSchema);

        services.AddScoped<IQueryAuthorizer, QueryAuthorizer>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();
    }

    public static void UseWolverine(WolverineOptions opts)
    {
        // Setting up Sql Server-backed message storage
        // This requires a reference to Wolverine.SqlServer
        opts.PersistMessagesWithSqlServer(_connectionString, _wolverineDbSchema);

        // Enrolling all local queues into the
        // durable inbox/outbox processing
        opts.Policies.UseDurableLocalQueues();

        // Add the auto transaction middleware attachment policy
        // If enabled, handlers don't need [AutoApplyTransactions] attribute.
        //opts.Policies.AutoApplyTransactions();
    }
}