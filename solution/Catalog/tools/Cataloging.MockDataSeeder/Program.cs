using Cataloging.Infrastructure.Database;
using Common.Application.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Cataloging.MockDataSeeder;

internal class Program
{
    [SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "No need.")]
    private static async Task Main()
    {
        var connectionString = "Data Source=(localdb)\\BookStore;Initial Catalog=Catalog;Integrated Security=True";

        Console.Write($"Do you want to seed Catalog database? (y/N): ");
        var answer = Console.ReadLine();

        if (answer == null || !answer.Equals("Y", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddDbContext<CatalogDbContext>(options => options.UseSqlServer(connectionString));
        serviceCollection.AddScoped<IUserService, SystemUserService>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Use the DbContext in your application
        using var dbContext = serviceProvider.GetRequiredService<CatalogDbContext>();
        await DataSeeder.SeedDataAsync(dbContext);
    }
}