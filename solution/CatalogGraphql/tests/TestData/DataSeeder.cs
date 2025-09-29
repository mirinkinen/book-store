using Infra.Data;

namespace TestData;

public static class DataSeeder
{
    public static async Task SeedDataAsync(CatalogDbContext catalogDbContext)
    {
        ArgumentNullException.ThrowIfNull(catalogDbContext);

        var authors = TestDataContainer.GetAuthors().ToList();

        // If not already seeded.
        if (!catalogDbContext.Authors.Any())
        {
            await catalogDbContext.AddRangeAsync(authors);
            await catalogDbContext.SaveChangesAsync();
        }

        // If not already seeded.
        if (!catalogDbContext.Books.Any())
        {
            var books = TestDataContainer.GetBooks(catalogDbContext.Authors.ToList());
            await catalogDbContext.AddRangeAsync(books);
            await catalogDbContext.SaveChangesAsync();
        }
        
        // If not already seeded.
        if (!catalogDbContext.Reviews.Any())
        {
            var books = TestDataContainer.GetReviews(catalogDbContext.Books.ToList());
            await catalogDbContext.AddRangeAsync(books);
            await catalogDbContext.SaveChangesAsync();
        }
    }
}