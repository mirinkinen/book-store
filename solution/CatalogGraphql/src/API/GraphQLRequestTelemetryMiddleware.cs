using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace API;

public class GraphQLRequestTelemetryMiddleware
{
    private readonly RequestDelegate _next;
    private readonly TelemetryClient _telemetryClient;
    private readonly ILogger<GraphQLRequestTelemetryMiddleware> _logger;

    public GraphQLRequestTelemetryMiddleware(
        RequestDelegate next,
        TelemetryClient telemetryClient,
        ILogger<GraphQLRequestTelemetryMiddleware> logger)
    {
        _next = next;
        _telemetryClient = telemetryClient;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Only process GraphQL requests
        if (!context.Request.Path.StartsWithSegments("/graphql"))
        {
            await _next(context);
            return;
        }

        var operationName = "GraphQL Request";
        var operation = _telemetryClient.StartOperation<RequestTelemetry>(operationName);

        try
        {
            operation.Telemetry.Properties["GraphQL.Endpoint"] = context.Request.Path;
            operation.Telemetry.Properties["GraphQL.Method"] = context.Request.Method;

            _logger.LogInformation("GraphQL request started: {Path}", context.Request.Path);

            await _next(context);

            operation.Telemetry.Success = context.Response.StatusCode < 400;
            operation.Telemetry.ResponseCode = context.Response.StatusCode.ToString();

            _logger.LogInformation("GraphQL request completed: {Path}, Status: {StatusCode}", 
                context.Request.Path, context.Response.StatusCode);
        }
        catch (Exception ex)
        {
            operation.Telemetry.Success = false;
            _telemetryClient.TrackException(ex);
            _logger.LogError(ex, "GraphQL request failed: {Path}", context.Request.Path);
            throw;
        }
        finally
        {
            _telemetryClient.StopOperation(operation);
        }
    }
}
