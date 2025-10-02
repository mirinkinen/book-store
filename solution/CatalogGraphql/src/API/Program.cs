namespace API;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Configure();

        var app = builder.Build();
        // Add Application Insights telemetry middleware for GraphQL requests
        app.UseMiddleware<GraphQLRequestTelemetryMiddleware>();
        app.MapGraphQL();
        
        // Executes command parameters like seed data etc. Exits the app when done.
        await ArgumentExecutor.ExecuteArguments(app, args);

        // Normal GraphQL server execution.
        await app.RunWithGraphQLCommandsAsync(args);
    }
}