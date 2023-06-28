using Cataloging.Domain.Authors;
using Cataloging.Domain.Books;

namespace Cataloging.MockDataSeeder;

public static class MockDataContainer
{
    private static readonly Guid _systemUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    private static readonly Random _randomizer = new(Guid.NewGuid().GetHashCode());
    public static Guid AuthorizedOrganization1 => Guid.Parse("5D8E6753-1479-408E-BB3D-CB3A02BE486C");
    public static Guid AuthorizedOrganization2 => Guid.Parse("284F633F-2D13-4F4D-8E37-1EE5C9F6B140");

    public static Guid StephenKingId { get; } = Guid.Parse("8E6A9434-87F5-46B2-A6C3-522DC35D8EEF");
    public static Guid DanBrownId { get; } = Guid.Parse("13E76BC5-BF2C-4FDB-BF42-8E0CA66EA7CE");
    public static Guid JkRowlingId { get; } = Guid.Parse("520B8C4F-72F1-4ECE-B1E2-8DD1DCA3476A");
    public static Guid WilliamShakeSpeareId { get; } = Guid.Parse("5321C585-9B2D-4A72-A105-122843E40E75");
    public static Guid ErnestHemingwayId { get; } = Guid.Parse("1668C115-23B2-40EF-BACC-CFB79F6DC391");

    public static IEnumerable<Author> GetAuthors()
    {
        var unauthorizedOrganization = Guid.Parse("A34B1695-DB25-4AFF-A717-3C47EC7E89F4");

        var authors = new Author[] {
            new Author("Stephen", "King", new DateTime(1947, 9, 21), AuthorizedOrganization1, _systemUserId).SetId(StephenKingId),
            new Author("Dan", "Brown", new DateTime(1964, 6, 22), AuthorizedOrganization1, _systemUserId).SetId(DanBrownId),
            new Author("J.K.", "Rowling", new DateTime(1965, 7, 31), AuthorizedOrganization2, _systemUserId).SetId(JkRowlingId),
            new Author("William", "Shakespeare", new DateTime(1564, 4, 15), AuthorizedOrganization2, _systemUserId).SetId(WilliamShakeSpeareId),
            new Author("Ernest.", "Hemingway", new DateTime(1899, 7, 21), unauthorizedOrganization, _systemUserId).SetId(ErnestHemingwayId)
        };

        return authors;
    }

    public static IEnumerable<Book> GetBooks(IEnumerable<Author> authors)
    {
        var authorsList = authors.ToList();

        var books = new List<Book>();

        // Add books with known IDs for easier testing.
        var theShining = new Book("The Shining", new DateTime(1977, 1, 28), StephenKingId, _systemUserId);
        theShining.SetId(Guid.Parse("A125C5BD-4F8E-4794-9C36-76E401FB4F24"));

        books.AddRange(new[] {
            theShining,
            new Book("The Green Mile", new DateTime(1996, 3, 28), StephenKingId, _systemUserId),
            new Book("End of Watch", new DateTime(2016, 6, 7), StephenKingId, _systemUserId),

            new Book("Angels and Demons", new DateTime(2000,5,15), DanBrownId, _systemUserId),
            new Book("The Da Vinci Code", new DateTime(2003, 3, 18), DanBrownId, _systemUserId),
            new Book("Inferno", new DateTime(2013, 5, 14), DanBrownId, _systemUserId),

            new Book("Harry Potter and the Philosopher's Stone", new DateTime(1997, 6,26), JkRowlingId, _systemUserId),
            new Book("Fantastic Beasts and Where to Find Them", new DateTime(2001, 3, 15), JkRowlingId, _systemUserId),
            new Book("Harry Potter and the Deathly Hallows", new DateTime(2007, 7, 21), JkRowlingId, _systemUserId),

            new Book("The Old Man and The Sea", new DateTime(1952, 1, 1), ErnestHemingwayId, _systemUserId),
            new Book("For Whom the Bell Tolls", new DateTime(1940, 1, 1), ErnestHemingwayId, _systemUserId),
            new Book("A Farewell to Arms", new DateTime(1929, 1, 1), ErnestHemingwayId, _systemUserId),
        });

        // Generate some random books for all authors.
        books.AddRange(Enumerable
            .Range(1, 1000)
            .Select(id => new Book($"Book #{id}", GetRandomPublishedDate(), GetRandomAuthor(authorsList).Id, _systemUserId)));

        return books;
    }

    private static Author GetRandomAuthor(IList<Author> authors)
    {
        return authors[_randomizer.Next(0, 4)];
    }

    private static DateTime GetRandomPublishedDate()
    {
        return DateTime.UtcNow - TimeSpan.FromDays(_randomizer.Next(0, 10000));
    }
}