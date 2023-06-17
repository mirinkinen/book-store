using Books.Api.Domain.Authors;
using Books.Api.Domain.Books;
using System.Linq;

namespace Books.Api.Infrastructure.Database;

public static class DataSeeder
{
    private static readonly Random _randomizer = new(Guid.NewGuid().GetHashCode());

    public static async Task SeedData(BooksDbContext booksDbContext)
    {
        ArgumentNullException.ThrowIfNull(booksDbContext);

        var stephen = new Author("Stephen", "King", new DateTime(1947, 9, 21));
        var dan = new Author("Dan", "Brown", new DateTime(1964, 6, 22));
        var jk = new Author("J.K.", "Rowling", new DateTime(1965, 7, 31));

        await booksDbContext.AddRangeAsync(stephen, dan, jk);
        await booksDbContext.SaveChangesAsync();

        var books = Enumerable.Range(1, 1000).Select(id => new Book($"Book #{id}", GetRandomPublishedDate(), stephen.Id));

        await booksDbContext.AddRangeAsync(books);
        await booksDbContext.SaveChangesAsync();
    }

    private static DateTime GetRandomPublishedDate()
    {
        return DateTime.UtcNow - TimeSpan.FromDays(_randomizer.Next(0, 10000));
    }
}