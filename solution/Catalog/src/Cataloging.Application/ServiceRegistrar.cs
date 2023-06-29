using Cataloging.Application.Requests.Books.GetBooks;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Auditing;
using Shared.Application.Authentication;

namespace Cataloging.Application;

public static class ServiceRegistrar
{
    public static void RegisterApplicationServices(IServiceCollection services)
    {        
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuditContext, AuditContext>();
    }
}