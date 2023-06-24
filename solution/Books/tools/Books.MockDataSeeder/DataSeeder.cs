using Books.Domain.Authors;
using Books.MockData;

namespace Books.Infrastructure.Database;

public static class DataSeeder
{
    private static readonly Lazy<IEnumerable<Author>> _authors = new(MockDataContainer.GetAuthors);

    public static async Task SeedData(BooksDbContext booksDbContext)
    {
        ArgumentNullException.ThrowIfNull(booksDbContext);

        // If not already seeded.
        if (!booksDbContext.Authors.Any())
        {
            await booksDbContext.AddRangeAsync(_authors.Value);
            await booksDbContext.SaveChangesAsync();
        };

        // If not already seeded.
        if (!booksDbContext.Books.Any())
        {
            var books = MockDataContainer.GetBooks(_authors.Value);
            await booksDbContext.AddRangeAsync(books);
            await booksDbContext.SaveChangesAsync();
        }
    }
}