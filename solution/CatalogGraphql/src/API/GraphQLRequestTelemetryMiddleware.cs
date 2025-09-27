using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System.Text.Json;
using System.Text;

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

        var (operationName, operationType, query) = await ExtractGraphQLOperationAsync(context);

        var telemetryOperationName = string.IsNullOrEmpty(operationName) 
            ? $"GraphQL {operationType}" 
            : $"GraphQL {operationType}: {operationName}";

        var operation = _telemetryClient.StartOperation<RequestTelemetry>(telemetryOperationName);

        try
        {
            // Add GraphQL-specific properties
            operation.Telemetry.Properties["GraphQL.Endpoint"] = context.Request.Path;
            operation.Telemetry.Properties["GraphQL.Method"] = context.Request.Method;
            operation.Telemetry.Properties["GraphQL.OperationType"] = operationType;

            if (!string.IsNullOrEmpty(operationName))
                operation.Telemetry.Properties["GraphQL.OperationName"] = operationName;

            if (!string.IsNullOrEmpty(query))
                operation.Telemetry.Properties["GraphQL.Query"] = TruncateQuery(query);

            _logger.LogInformation("GraphQL {OperationType} started: {OperationName}", 
                operationType, operationName ?? "Anonymous");

            await _next(context);

            operation.Telemetry.Success = context.Response.StatusCode < 400;
            operation.Telemetry.ResponseCode = context.Response.StatusCode.ToString();

            _logger.LogInformation("GraphQL {OperationType} completed: {OperationName}, Status: {StatusCode}", 
                operationType, operationName ?? "Anonymous", context.Response.StatusCode);
        }
        catch (Exception ex)
        {
            operation.Telemetry.Success = false;
            _telemetryClient.TrackException(ex, new Dictionary<string, string>
            {
                ["GraphQL.OperationType"] = operationType,
                ["GraphQL.OperationName"] = operationName ?? "Anonymous"
            });
            _logger.LogError(ex, "GraphQL {OperationType} failed: {OperationName}", 
                operationType, operationName ?? "Anonymous");
            throw;
        }
        finally
        {
            _telemetryClient.StopOperation(operation);
        }
    }

    private async Task<(string operationName, string operationType, string query)> ExtractGraphQLOperationAsync(HttpContext context)
    {
        try
        {
            if (context.Request.Method == "GET")
            {
                var query = context.Request.Query["query"].ToString();
                var operationName = context.Request.Query["operationName"].ToString();
                return (operationName, DetectOperationType(query), query);
            }
            else if (context.Request.Method == "POST")
            {
                // Enable buffering to read the body multiple times
                context.Request.EnableBuffering();

                using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
                var body = await reader.ReadToEndAsync();

                // Reset the body position for the next middleware
                context.Request.Body.Position = 0;

                if (string.IsNullOrEmpty(body))
                    return ("", "Unknown", "");

                var request = JsonSerializer.Deserialize<GraphQLRequest>(body, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var operationType = DetectOperationType(request?.Query ?? "");
                return (request?.OperationName ?? "", operationType, request?.Query ?? "");
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to parse GraphQL request");
        }

        return ("", "Unknown", "");
    }

    private string DetectOperationType(string query)
    {
        if (string.IsNullOrEmpty(query))
            return "Unknown";

        var trimmedQuery = query.Trim();
        if (trimmedQuery.StartsWith("mutation", StringComparison.OrdinalIgnoreCase))
            return "Mutation";
        if (trimmedQuery.StartsWith("subscription", StringComparison.OrdinalIgnoreCase))
            return "Subscription";
        if (trimmedQuery.StartsWith("query", StringComparison.OrdinalIgnoreCase))
            return "Query";

        // Default to Query if no explicit type is specified
        return "Query";
    }

    private string TruncateQuery(string query, int maxLength = 1000)
    {
        if (string.IsNullOrEmpty(query) || query.Length <= maxLength)
            return query;

        return query.Substring(0, maxLength) + "...";
    }

    private class GraphQLRequest
    {
        public string? Query { get; set; }
        public string? OperationName { get; set; }
        public JsonElement? Variables { get; set; }
    }
}
