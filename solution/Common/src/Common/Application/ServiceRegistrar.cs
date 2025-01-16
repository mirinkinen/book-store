using Common.Api.Application.Auditing;
using Common.Api.Application.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Wolverine;

namespace Common.Api.Application;

public static class ServiceRegistrar
{
    public static void RegisterApplicationServices(IServiceCollection services)
    {
        // User
        services.AddScoped<IUserService, UserService>();

        // Audit.
        services.AddScoped<AuditContext>();
    }

    public static void UseCommonApplicationSettings(this WolverineOptions opts)
    {
        opts.Policies.LogMessageStarting(LogLevel.Debug);
        opts.Discovery.IncludeAssembly(typeof(AuditLogEventHandler).Assembly);
    }
}