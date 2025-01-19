using Common.Application.Auditing;
using Common.Application.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Wolverine;
using Wolverine.FluentValidation;

namespace Common.Application;

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
        opts.UseFluentValidation();
         // opts.UseFluentValidationProblemDetailMiddleware();

        
        opts.Policies.LogMessageStarting(LogLevel.Debug);
        opts.Discovery.IncludeAssembly(typeof(AuditLogEventHandler).Assembly);
    }
}