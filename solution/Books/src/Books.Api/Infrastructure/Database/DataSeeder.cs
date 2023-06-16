using Books.Api.Domain.Authors;

namespace Books.Api.Infrastructure.Database;

public static class DataSeeder
{
    public static async Task SeedData(BooksDbContext booksDbContext)
    {
        ArgumentNullException.ThrowIfNull(booksDbContext);

        await booksDbContext.AddRangeAsync(
            new Author("Stephen", "King", new DateTime(1947, 9, 21)),
            new Author("Dan", "Brown", new DateTime(1964, 6, 22)),
            new Author("J.K.", "Rowling", new DateTime(1965, 7, 31)));

        await booksDbContext.SaveChangesAsync();
    }
}
