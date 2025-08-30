using API.Extensions;
using API.Types;
using Application.Repositories;
using Infra.Data;
using Infra.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure GraphQL with a single Query type containing all operations
builder.AddGraphQL()
    .AddTypes();

// Configure EF Core
builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();

var app = builder.Build();

// Initialize database and apply migrations
if (args.Length == 0) // Only apply migrations when not running migrations command
{
    await app.InitializeDatabaseAsync();
}

app.MapGraphQL();
app.RunWithGraphQLCommands(args);