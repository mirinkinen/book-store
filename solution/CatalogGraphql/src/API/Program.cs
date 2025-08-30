using API.Types;
using Application.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configure GraphQL with a single Query type containing all operations
builder.AddGraphQL()
    .AddTypes();

// Register repositories
builder.Services.AddSingleton<IBookRepository, BookRepository>();
builder.Services.AddSingleton<IAuthorRepository, AuthorRepository>();

var app = builder.Build();
app.MapGraphQL();
app.RunWithGraphQLCommands(args);