using Cataloging.Application.Auditing;
using Common.Application.Authentication;
using System.Reflection;
using Wolverine;

namespace Cataloging.Application;

public static class ServiceConfigurator
{
    public static void ConfigureApplicationServices(IServiceCollection services)
    {
        // User
        services.AddScoped<IUserAccessor, UserAccessor>();

        // Audit.
        services.AddScoped<AuditContext>();
    }

    public static void UseCommonWolverineApplicationSettings(this WolverineOptions opts)
    {
        opts.Policies.LogMessageStarting(LogLevel.Debug);
        opts.Discovery.IncludeAssembly(Assembly.GetExecutingAssembly());
    }
}