using Application.AuthorMutations.CreateAuthor;
using System.Collections.Immutable;

namespace API;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Configure();

        var app = builder.Build();
        app.MapGraphQL();

        // Executes command parameters like seed data etc. Exits the app when done.
        await ArgumentExecutor.ExecuteArguments(app, args);

        // Normal GraphQL server execution.
        await app.RunWithGraphQLCommandsAsync(args);
    }
}