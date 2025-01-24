using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Trace;
using Wolverine;

namespace Common;

public static class ServiceConfigurator
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
        
        builder.Services.AddFluentValidationAutoValidation(config =>
        {
            config.DisableDataAnnotationsValidation = true;
        });

        // Disable default member name manipulation.
        ValidatorOptions.Global.DisplayNameResolver = (type, memberInfo, expression) => memberInfo?.Name ?? "";
    }
}