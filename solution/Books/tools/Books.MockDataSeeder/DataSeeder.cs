using Books.MockData;

namespace Books.Infrastructure.Database;

public static class DataSeeder
{
    public static async Task SeedData(BooksDbContext booksDbContext)
    {
        ArgumentNullException.ThrowIfNull(booksDbContext);

        var authors = MockDataContainer.GetAuthors();

        // If not already seeded.
        if (!booksDbContext.Authors.Any())
        {
            await booksDbContext.AddRangeAsync(authors);
            await booksDbContext.SaveChangesAsync();
        };

        // If not already seeded.
        if (!booksDbContext.Books.Any())
        {
            var books = MockDataContainer.GetBooks(authors);
            await booksDbContext.AddRangeAsync(books);
            await booksDbContext.SaveChangesAsync();
        }
    }
}