using Microsoft.Extensions.DependencyInjection;
using Common.Application.Auditing;
using Common.Application.Authentication;

namespace Cataloging.Application;

public static class ServiceRegistrar
{
    public static void RegisterApplicationServices(IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuditContext, AuditContext>();
    }
}