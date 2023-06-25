using Books.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace DatabaseSeeder;

internal class Program
{
    [SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "No need.")]
    private static async Task Main()
    {
        var connectionString = "Data Source=(localdb)\\BookStore;Initial Catalog=BookStore;Integrated Security=True";

        Console.Write($"Do you want to seed the database (localdb)\\BookStore? (y/N): ");
        var answer = Console.ReadLine();

        if (answer == null || !answer.Equals("Y", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var serviceProvider = new ServiceCollection()
          .AddDbContext<BooksDbContext>(options => options.UseSqlServer(connectionString))
          .BuildServiceProvider();

        // Use the DbContext in your application
        using var dbContext = serviceProvider.GetRequiredService<BooksDbContext>();
        await DataSeeder.SeedDataAsync(dbContext);
    }
}