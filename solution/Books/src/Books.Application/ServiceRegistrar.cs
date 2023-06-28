using Books.Application.Auditing;
using Books.Application.Requests.Books.GetBooks;
using Books.Application.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Books.Application;

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