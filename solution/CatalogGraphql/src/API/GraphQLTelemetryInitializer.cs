using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace API;

public class GraphQLTelemetryInitializer : ITelemetryInitializer
{
    public void Initialize(ITelemetry telemetry)
    {
        // Add common properties to all telemetry
        telemetry.Context.Component.Version = typeof(Program).Assembly.GetName().Version?.ToString() ?? "Unknown";
        telemetry.Context.Properties["Service"] = "CatalogGraphQL";

        // Add cloud role name for better Application Insights experience
        telemetry.Context.Cloud.RoleName = "catalog-graphql-api";
    }
}