var builder = WebApplication.CreateBuilder(args);

// Configure GraphQL with a single Query type containing all operations
builder.ConfigureGraphQL();
builder.ConfigureEFCore();
builder.ConfigureInfraServices();

var app = builder.Build();
app.MapGraphQL();

// Executes command parameters like seed data etc. Exists the app when done.
await ArgumentExecutor.ExecuteArguments(app, args);

// Normal GraphQL server execution.
await app.RunWithGraphQLCommandsAsync(args);