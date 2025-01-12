using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Trace;
using Wolverine;

namespace Common.Api;

public static class ServiceRegistrar
{
    public static void UseCommonApiSettings(this WolverineOptions opts, WebApplicationBuilder builder)
    {
        opts.Services.AddOpenTelemetry()
            .WithTracing(trace => trace
                .AddSource("*")
                .AddAspNetCoreInstrumentation()
                .AddConsoleExporter());

        if (builder.Environment.IsDevelopment())
        {
            opts.Durability.Mode = DurabilityMode.Solo;
        }
        
        // Enable to preview generated code upon first call.
        //opts.CodeGeneration.TypeLoadMode = JasperFx.CodeGeneration.TypeLoadMode.Auto;
    }
}