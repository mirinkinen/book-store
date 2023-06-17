using Books.Api.Application;
using Books.Api.Domain.Authors;
using Books.Api.Domain.Books;

namespace Books.Api.Infrastructure.Database;

public static class DataSeeder
{
    private static readonly Random _randomizer = new(Guid.NewGuid().GetHashCode());

    public static async Task SeedData(BooksDbContext booksDbContext)
    {
        ArgumentNullException.ThrowIfNull(booksDbContext);

        var user = new User();
        var authorizedOrganization1 = user.Organizations.First();
        var authorizedOrganization2 = user.Organizations.Last();
        var unauthorizedOrganization = Guid.Parse("A34B1695-DB25-4AFF-A717-3C47EC7E89F4");

        var authors = new[] {
            new Author("Stephen", "King", new DateTime(1947, 9, 21), authorizedOrganization1),
            new Author("Dan", "Brown", new DateTime(1964, 6, 22), authorizedOrganization1),
            new Author("J.K.", "Rowling", new DateTime(1965, 7, 31), authorizedOrganization2),
            new Author("Ernest.", "Hemingway", new DateTime(1899, 7, 21), unauthorizedOrganization)
        };

        // Set explicit IDs for easier testing.
        authors[0].Id = Guid.Parse("8E6A9434-87F5-46B2-A6C3-522DC35D8EEF");
        authors[1].Id = Guid.Parse("13E76BC5-BF2C-4FDB-BF42-8E0CA66EA7CE");
        authors[2].Id = Guid.Parse("520B8C4F-72F1-4ECE-B1E2-8DD1DCA3476A");
        authors[3].Id = Guid.Parse("1668C115-23B2-40EF-BACC-CFB79F6DC391");

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
        return authors[_randomizer.Next(0, 4)];
    }
}