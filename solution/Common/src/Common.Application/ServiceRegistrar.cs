using Common.Application.Auditing;
using Common.Application.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Wolverine;
using Wolverine.ErrorHandling;

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
    
    public static void UseWolferine(WolverineOptions opts)
    {
        opts.LocalQueue("durable").UseDurableInbox();
        
        opts.Policies.OnAnyException().RetryWithCooldown(
            TimeSpan.FromSeconds(1),
            TimeSpan.FromSeconds(5),
            TimeSpan.FromSeconds(10));
    }
}