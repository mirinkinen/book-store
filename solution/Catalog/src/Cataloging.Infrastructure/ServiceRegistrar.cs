using Cataloging.Application.Services;
using Cataloging.Domain.Authors;
using Cataloging.Infrastructure.Database;
using Cataloging.Infrastructure.Queries;
using Cataloging.Infrastructure.Repository;
using Common.Application.Messages;
using Microsoft.Extensions.DependencyInjection;
using Wolverine;
using Wolverine.Transports.Tcp;

namespace Cataloging.Infrastructure;

public static class ServiceRegistrar
{
    public static void RegisterInfrastructureServices(IServiceCollection services, string connectionString)
    {
        Common.Infrastructure.ServiceRegistrar.RegisterInfrastructureServices<CatalogDbContext>(services,
            connectionString);

        services.AddScoped<IQueryAuthorizer, QueryAuthorizer>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();
    }

    public static void UseWolverine(WolverineOptions opts, string connectionString)
    {
        opts.ListenAtPort(5201).UseDurableInbox();
        opts.PublishMessage<Pong>().ToPort(5202).UseDurableOutbox();

        Common.Infrastructure.ServiceRegistrar.UseWolverine(opts, connectionString);
    }
}