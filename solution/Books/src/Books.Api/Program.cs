using Books.Api.Infrastructure.Database;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

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
    }

    private static void AddApplicationServices(WebApplicationBuilder builder)
    {
        //builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<>());
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