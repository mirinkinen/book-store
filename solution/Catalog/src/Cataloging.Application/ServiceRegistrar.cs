using Microsoft.Extensions.DependencyInjection;
using Common.Application.Auditing;
using Common.Application.Authentication;

namespace Cataloging.Application;

public static class ServiceRegistrar
{
    public static void RegisterApplicationServices(IServiceCollection services)
    {
        // User
        services.AddScoped<IUserService, UserService>();

        // Audit.
        services.AddScoped<IAuditContext, AuditContext>();
        services.AddSingleton<IAuditContextPublisher, AuditContextPublisher>();
    }
}