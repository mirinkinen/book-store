using Cataloging.Application.Auditing;
using Cataloging.Application.Requests.Books.GetBooks;
using Cataloging.Application.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

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