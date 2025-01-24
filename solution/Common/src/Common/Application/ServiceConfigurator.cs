using Common.Application.Auditing;
using Common.Application.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Wolverine;

namespace Common.Application;

public static class ServiceConfigurator
{
    public static void ConfigureApplicationServices(IServiceCollection services)
    {
        // User
        services.AddScoped<IUserService, UserService>();

        // Audit.
        services.AddScoped<AuditContext>();
    }

    public static void UseCommonWolverineApplicationSettings(this WolverineOptions opts)
    {
        opts.Policies.LogMessageStarting(LogLevel.Debug);
        opts.Discovery.IncludeAssembly(typeof(AuditLogEventHandler).Assembly);
    }
}