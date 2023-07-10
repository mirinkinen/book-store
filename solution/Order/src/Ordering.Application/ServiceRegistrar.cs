using Common.Application.Auditing;
using Common.Application.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Application;

public static class ServiceRegistrar
{
    public static void RegisterApplicationServices(IServiceCollection services)
    {
        // User
        services.AddScoped<IUserService, UserService>();

        // Audit.
        services.AddScoped<AuditContext>();
    }
}