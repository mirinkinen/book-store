namespace Infra.Data.Seed;

public static class DataSeeder
{
    public static async Task SeedDataAsync(CatalogDbContext catalogDbContext)
    {
        ArgumentNullException.ThrowIfNull(catalogDbContext);

        var authors = MockDataContainer.GetAuthors().ToList();

        // If not already seeded.
        if (!catalogDbContext.Authors.Any())
        {
            await catalogDbContext.AddRangeAsync(authors);
            await catalogDbContext.SaveChangesAsync();
        }

        // If not already seeded.
        if (!catalogDbContext.Books.Any())
        {
            var books = MockDataContainer.GetBooks(authors);
            await catalogDbContext.AddRangeAsync(books);
            await catalogDbContext.SaveChangesAsync();
        }
    }
}