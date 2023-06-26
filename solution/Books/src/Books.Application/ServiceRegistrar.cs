using Books.Application.Requests.Books.GetBooks;
using Books.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Books.Application;

public static class ServiceRegistrar
{
    public static void RegisterApplicationServices(IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetBooksQuery>());

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEntityAuditor, EntityAuditor>();
    }
}