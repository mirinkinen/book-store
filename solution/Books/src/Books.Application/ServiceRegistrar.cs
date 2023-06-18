using Books.Application.Requests.Books.GetBooks;
using Microsoft.Extensions.DependencyInjection;

namespace Books.Application;

public static class ServiceRegistrar
{
    public static void RegisterApplicationServices(IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetBooksQuery>());
        services.AddScoped<UserService>();
    }
}