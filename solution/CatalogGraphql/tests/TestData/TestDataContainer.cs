using Common.Infra;
using Domain;

namespace TestData;

public static class TestDataContainer
{
    public static Guid SystemUserId => Guid.Parse("11111111-1111-1111-1111-111111111111");

    public static Guid AuthorizedOrganization1 => Guid.Parse("5D8E6753-1479-408E-BB3D-CB3A02BE486C");
    public static Guid AuthorizedOrganization2 => Guid.Parse("284F633F-2D13-4F4D-8E37-1EE5C9F6B140");

    // Author IDs.
    public static Guid StephenKingId => Guid.Parse("8E6A9434-87F5-46B2-A6C3-522DC35D8EEF");
    public static Guid DanBrownId => Guid.Parse("13E76BC5-BF2C-4FDB-BF42-8E0CA66EA7CE");
    public static Guid JkRowlingId => Guid.Parse("520B8C4F-72F1-4ECE-B1E2-8DD1DCA3476A");
    public static Guid WilliamShakeSpeareId => Guid.Parse("5321C585-9B2D-4A72-A105-122843E40E75");
    public static Guid ErnestHemingwayId => Guid.Parse("1668C115-23B2-40EF-BACC-CFB79F6DC391");

    // Book IDs.
    public static Guid HarryPotterAndTheDeathlyHallows => Guid.Parse("6F6D9786-074C-4828-8DDD-5852A9530203");

    public static Guid TheShiningId => Guid.Parse("A125C5BD-4F8E-4794-9C36-76E401FB4F24");

    // Review IDs.
    public static Guid ShiningReview1Id => Guid.Parse("B1234567-1234-1234-1234-123456789001");
    public static Guid ShiningReview2Id => Guid.Parse("B1234567-1234-1234-1234-123456789002");
    public static Guid DaVinciCodeReviewId => Guid.Parse("B1234567-1234-1234-1234-123456789003");
    public static Guid HarryPotterReviewId => Guid.Parse("B1234567-1234-1234-1234-123456789004");

    public static IEnumerable<Author> GetAuthors()
    {
        var unauthorizedOrganization = Guid.Parse("A34B1695-DB25-4AFF-A717-3C47EC7E89F4");

        var authors = new Author[]
        {
            new Author("Stephen", "King", new DateOnly(1947, 9, 21), AuthorizedOrganization1).SetId(StephenKingId),
            new Author("Dan", "Brown", new DateOnly(1964, 6, 22), AuthorizedOrganization1).SetId(DanBrownId),
            new Author("J.K.", "Rowling", new DateOnly(1965, 7, 31), AuthorizedOrganization2).SetId(JkRowlingId),
            new Author("William", "Shakespeare", new DateOnly(1564, 4, 15), AuthorizedOrganization2).SetId(
                WilliamShakeSpeareId),
            new Author("Ernest.", "Hemingway", new DateOnly(1899, 7, 21), unauthorizedOrganization).SetId(
                ErnestHemingwayId)
        };

        return authors;
    }

    public static IEnumerable<Book> GetBooks(IEnumerable<Author> authors)
    {
        var authorsList = authors.ToList();

        var books = new List<Book>();

        // Add books with known IDs for easier testing.
        var theShining = new Book(StephenKingId, "The Shining", new DateOnly(1977, 1, 28), 20);
        theShining.SetId(TheShiningId);

        books.AddRange(new[]
        {
            theShining,
            new Book(StephenKingId, "The Green Mile", new DateOnly(1996, 3, 28), 20),
            new Book(StephenKingId, "End of Watch", new DateOnly(2016, 6, 7), 25),

            new Book(DanBrownId, "Angels and Demons", new DateOnly(2000, 5, 15), 30),
            new Book(DanBrownId, "The Da Vinci Code", new DateOnly(2003, 3, 18), 35),
            new Book(DanBrownId, "Inferno", new DateOnly(2013, 5, 14), 33),

            new Book(JkRowlingId, "Harry Potter and the Philosopher's Stone", new DateOnly(1997, 6, 26), 15),
            new Book(JkRowlingId, "Fantastic Beasts and Where to Find Them", new DateOnly(2001, 3, 15), 17),
            new Book(JkRowlingId, "Harry Potter and the Deathly Hallows", new DateOnly(2007, 7, 21), 21)
                .SetId(HarryPotterAndTheDeathlyHallows),

            new Book(ErnestHemingwayId, "The Old Man and The Sea", new DateOnly(1952, 1, 1), 15),
            new Book(ErnestHemingwayId, "For Whom the Bell Tolls", new DateOnly(1940, 1, 1), 14),
            new Book(ErnestHemingwayId, "A Farewell to Arms", new DateOnly(1929, 1, 1), 13),
        });

        // Generate deterministic books for all authors.
        books.AddRange(Enumerable
            .Range(1, 1000)
            .Select(id => new Book(GetDeterministicAuthor(authorsList, id).Id, $"Book #{id}", GetDeterministicPublishedDate(id),
                GetDeterministicPrice(id))));

        return books;
    }

    private static decimal GetDeterministicPrice(int bookId)
    {
        // Use modulo to create deterministic price between 10 and 49
        return 10 + bookId % 40;
    }

    private static Author GetDeterministicAuthor(List<Author> authors, int bookId)
    {
        // Use modulo to deterministically assign authors based on book ID
        return authors[bookId % authors.Count];
    }

    private static DateOnly GetDeterministicPublishedDate(int bookId)
    {
        // Use modulo to create deterministic dates, cycling through past 10000 days
        return DateOnly.FromDateTime(DateTime.UtcNow - TimeSpan.FromDays(bookId % 10000));
    }

    public static IEnumerable<Review> GetReviews(IEnumerable<Book> books)
    {
        var booksList = books.ToList();
        var reviews = new List<Review>();

        // Add specific reviews with known IDs for easier testing
        var theShiningId = Guid.Parse("A125C5BD-4F8E-4794-9C36-76E401FB4F24");
        var daVinciCodeBook = booksList.FirstOrDefault(b => b.Title == "The Da Vinci Code");
        var harryPotterBook = booksList.FirstOrDefault(b => b.Id == HarryPotterAndTheDeathlyHallows);

        reviews.AddRange(new[]
        {
            new Review
            {
                Id = ShiningReview1Id,
                BookId = theShiningId,
                Title = "Terrifyingly Good",
                Body =
                    "Stephen King's masterpiece of horror. The psychological descent into madness is brilliantly portrayed. A must-read for horror fans.",
                CreatedAt = DateTime.UtcNow.AddDays(-30),
                ModifiedAt = DateTime.UtcNow.AddDays(-30),
                ModifiedBy = SystemUserId
            },
            new Review
            {
                Id = ShiningReview2Id,
                BookId = theShiningId,
                Title = "Classic Horror",
                Body = "One of the best horror novels ever written. The atmosphere and tension build perfectly throughout the story.",
                CreatedAt = DateTime.UtcNow.AddDays(-15),
                ModifiedAt = DateTime.UtcNow.AddDays(-15),
                ModifiedBy = SystemUserId
            }
        });

        if (daVinciCodeBook != null)
        {
            reviews.Add(new Review
            {
                Id = DaVinciCodeReviewId,
                BookId = daVinciCodeBook.Id,
                Title = "Engaging Mystery",
                Body = "Dan Brown weaves an intricate tale of mystery and conspiracy. Fast-paced and hard to put down.",
                CreatedAt = DateTime.UtcNow.AddDays(-45),
                ModifiedAt = DateTime.UtcNow.AddDays(-45),
                ModifiedBy = SystemUserId
            });
        }

        if (harryPotterBook != null)
        {
            reviews.Add(new Review
            {
                Id = HarryPotterReviewId,
                BookId = harryPotterBook.Id,
                Title = "Epic Conclusion",
                Body = "The final book in the Harry Potter series delivers an emotional and satisfying conclusion to the beloved series.",
                CreatedAt = DateTime.UtcNow.AddDays(-60),
                ModifiedAt = DateTime.UtcNow.AddDays(-60),
                ModifiedBy = SystemUserId
            });
        }

        // Generate deterministic reviews for random books
        reviews.AddRange(Enumerable
            .Range(1, 100)
            .Select(id => new Review
            {
                BookId = GetDeterministicBook(booksList, id).Id,
                Title = GetDeterministicReviewTitle(id),
                Body = GetDeterministicReviewBody(id),
                CreatedAt = DateTime.UtcNow.AddDays(-GetDeterministicDaysAgo(id)),
                ModifiedAt = DateTime.UtcNow.AddDays(-GetDeterministicDaysAgo(id)),
                ModifiedBy = SystemUserId
            }));

        return reviews;
    }

    private static Book GetDeterministicBook(List<Book> books, int reviewId)
    {
        // Use modulo to deterministically assign books based on review ID
        return books[reviewId % books.Count];
    }

    private static string GetDeterministicReviewTitle(int reviewId)
    {
        var titles = new[]
        {
            "Amazing Read", "Loved It", "Great Book", "Highly Recommended", "Fantastic Story",
            "Well Written", "Engaging Plot", "Couldn't Put It Down", "Brilliant Work", "Masterpiece"
        };
        return titles[reviewId % titles.Length];
    }

    private static string GetDeterministicReviewBody(int reviewId)
    {
        var bodies = new[]
        {
            "This book exceeded my expectations. The character development was excellent and the plot kept me engaged throughout.",
            "A truly remarkable piece of literature. The author's writing style is captivating and the story is unforgettable.",
            "I thoroughly enjoyed reading this book. The pacing was perfect and the ending was satisfying.",
            "One of the best books I've read this year. Highly recommend to anyone looking for a great story.",
            "The author has created a compelling narrative that draws you in from the first page.",
            "Beautifully written with complex characters and an intricate plot. A must-read.",
            "This book offers profound insights while maintaining an engaging storyline throughout.",
            "The storytelling is exceptional and the themes are thought-provoking and relevant.",
            "A page-turner that combines excellent writing with a fascinating story.",
            "Outstanding work that showcases the author's talent and creativity."
        };
        return bodies[reviewId % bodies.Length];
    }

    private static int GetDeterministicDaysAgo(int reviewId)
    {
        // Use modulo to create deterministic review dates within the past year
        return 1 + reviewId % 365;
    }
}