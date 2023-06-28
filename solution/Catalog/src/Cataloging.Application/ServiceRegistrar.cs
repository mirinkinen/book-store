using Cataloging.Application.Requests.Books.GetBooks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Auditing;
using Shared.Application.Authentication;

namespace Cataloging.Application;

public static class ServiceRegistrar
{
    public static void RegisterApplicationServices(IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<GetBooksQuery>();
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuditableQueryBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuditableCommandBehaviour<,>));
        });

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuditContext, AuditContext>();
    }
}