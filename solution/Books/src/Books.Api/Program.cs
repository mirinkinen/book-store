using Books.Application;
using Books.Application.Requests.GetBooks;
using Books.Domain.Authors;
using Books.Domain.Books;
using Books.Infrastructure.Database;
using Microsoft.AspNetCore.OData;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using System.Reflection;

namespace Books.Api;

public static class Program
{
    private static SqliteConnection? _connection;

    public static async Task Main(string[] args)
    {
        IEnumerable<string> strings = Enumerable.Empty<string>();

        var builder = WebApplication.CreateBuilder(args);

        AddApiServices(builder);
        AddApplicationServices(builder);
        AddInfrastructureServices(builder);

        var app = builder.Build();

        await InitializeDatabase(app.Services);

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        await app.RunAsync();
    }

    private static void AddApiServices(WebApplicationBuilder builder)
    {
        // Add services to the container.
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        BuildODataModel(builder);
    }

    private static void BuildODataModel(WebApplicationBuilder builder)
    {
        var modelBuilder = new ODataConventionModelBuilder();

        var authorEntity = modelBuilder.EntityType<Author>();
        authorEntity.HasKey(book => book.Id);
        authorEntity.Property(book => book.Birthday);
        authorEntity.Property(book => book.Created);
        authorEntity.Property(book => book.Firstname);
        authorEntity.Property(book => book.Lastname);
        authorEntity.Property(book => book.Updated);
        authorEntity.ContainsMany(author => author.Books);
        var authorEntitySet = modelBuilder.EntitySet<Author>("Authors");

        var bookEntity = modelBuilder.EntityType<Book>();
        bookEntity.HasKey(book => book.Id);
        bookEntity.Property(book => book.AuthorId);
        bookEntity.Property(book => book.Created);
        bookEntity.Property(book => book.DatePublished);
        bookEntity.Property(book => book.Title);
        bookEntity.Property(book => book.Updated);
        bookEntity.ContainsRequired(book => book.Author);

        var bookEntitySet = modelBuilder.EntitySet<Book>("Books");

        builder.Services.AddControllers().AddOData(
            options => options
            .Select()
            .Filter()
            .OrderBy()
            .Expand()
            .Count()
            .SetMaxTop(100)
            .AddRouteComponents("odata", modelBuilder.GetEdmModel()));
    }

    private static void AddApplicationServices(WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetBooksQuery>());
        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<IQueryAuthorizer, QueryAuthorizer>();
    }

    private static void AddInfrastructureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<BooksDbContext>(dbContextOptions =>
        {
#pragma warning disable CS8604 // Possible null reference argument.
            dbContextOptions.UseSqlite(_connection);
#pragma warning restore CS8604 // Possible null reference argument.
        });
    }

    private static async Task InitializeDatabase(IServiceProvider services)
    {
        // Keep connection open or the in-memory database will be gone.
        _connection = new SqliteConnection("datasource=:memory:");
        await _connection.OpenAsync();

        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BooksDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
        await dbContext.Database.MigrateAsync();
        await DataSeeder.SeedData(dbContext);
    }
}