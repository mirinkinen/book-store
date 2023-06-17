using Books.Api.Domain.Authors;
using Books.Api.Domain.Books;

namespace Books.Api.Infrastructure.Database;

public static class DataSeeder
{
    private static readonly Random _randomizer = new(Guid.NewGuid().GetHashCode());

    public static async Task SeedData(BooksDbContext booksDbContext)
    {
        ArgumentNullException.ThrowIfNull(booksDbContext);

        var authors = new[] {
            new Author("Stephen", "King", new DateTime(1947, 9, 21)),
            new Author("Dan", "Brown", new DateTime(1964, 6, 22)),
            new Author("J.K.", "Rowling", new DateTime(1965, 7, 31))
        };

        await booksDbContext.AddRangeAsync(authors);
        await booksDbContext.SaveChangesAsync();
        
        var books = Enumerable
            .Range(1, 1000)
            .Select(id => new Book($"Book #{id}", GetRandomPublishedDate(), GetRandomAuthor(authors).Id));

        await booksDbContext.AddRangeAsync(books);
        await booksDbContext.SaveChangesAsync();
    }

    private static DateTime GetRandomPublishedDate()
    {
        return DateTime.UtcNow - TimeSpan.FromDays(_randomizer.Next(0, 10000));
    }

    private static Author GetRandomAuthor(Author[] authors)
    {
        return authors[_randomizer.Next(0, 3)];
    }
}