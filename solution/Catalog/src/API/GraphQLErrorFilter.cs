using Microsoft.ApplicationInsights;

namespace API;

public class GraphQLErrorFilter : IErrorFilter
{
    private readonly ILogger<GraphQLErrorFilter> _logger;
    private readonly TelemetryClient _telemetryClient;

    public GraphQLErrorFilter(ILogger<GraphQLErrorFilter> logger, TelemetryClient telemetryClient)
    {
        _logger = logger;
        _telemetryClient = telemetryClient;
    }

    public IError OnError(IError error)
    {
        // Don't log user errors (validation errors, business logic errors)
        if (error.Exception == null)
        {
            return error;
        }

        // Log the error with Application Insights
        var properties = new Dictionary<string, string>
        {
            ["GraphQL.FieldPath"] = error.Path?.ToString() ?? "Unknown",
            ["GraphQL.Code"] = error.Code ?? "UNKNOWN_ERROR",
            ["GraphQL.Message"] = error.Message
        };

        // Add operation context if available
        if (error.Extensions?.TryGetValue("operation", out var operation) == true)
        {
            properties["GraphQL.Operation"] = operation.ToString() ?? "";
        }

        if (error.Extensions?.TryGetValue("operationName", out var operationName) == true)
        {
            properties["GraphQL.OperationName"] = operationName.ToString() ?? "";
        }

        // Track exception with Application Insights
        _telemetryClient.TrackException(error.Exception, properties);

        // Log with structured logging
        _logger.LogError(error.Exception,
            "GraphQL Error: {Message} at {Path} with code {Code}",
            error.Message,
            error.Path?.ToString() ?? "Unknown",
            error.Code ?? "UNKNOWN_ERROR");

        // Return the error (optionally modify it to hide sensitive information in production)
        return error.WithMessage($"An error occurred while processing your request.");
    }
}