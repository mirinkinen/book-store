using Azure.Monitor.OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace API;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder Configure(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetValue<string>("APPLICATIONINSIGHTS_CONNECTION_STRING");

        builder.Logging.AddOpenTelemetry(b =>
            {
                b.IncludeFormattedMessage = true;
                b.IncludeScopes = true;
                b.ParseStateValues = true;
                b.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("GraphQLDemo"));

                if (!string.IsNullOrWhiteSpace(connectionString))
                {
                    b.AddAzureMonitorLogExporter();
                }
            }
        );
        
        builder.Services.RegisterServices(builder.Configuration);

        return builder;
    }
}