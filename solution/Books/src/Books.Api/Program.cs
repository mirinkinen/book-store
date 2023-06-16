using Books.Api.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Books.Api;

public static class Program
{
    public static async Task Main(string[] args)
    {
        IEnumerable<string> strings = Enumerable.Empty<string>();

        var builder = WebApplication.CreateBuilder(args);

        AddApiServices(builder);
        AddApplicationServices(builder);
        AddInfrastructureServices(builder);

        var app = builder.Build();

        await SeedData(app.Services);

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
        builder.Services.AddDbContextPool<BooksDbContext>(dbContextOptions =>
        {
            dbContextOptions.UseSqlite("Data Source=:memory:;");
        });
    }

    private static Task SeedData(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BooksDbContext>();

        return dbContext.Database.MigrateAsync();
    }
}