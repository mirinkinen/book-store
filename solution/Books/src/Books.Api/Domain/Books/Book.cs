using Books.Api.Domain.SeedWork;

namespace Books.Api.Domain.Books
{
    public class Book : Entity
    {
        public string Title { get; internal set; }

        public DateTime DatePublished { get; internal set; }

        public Guid AuthorId { get; internal set; }

        public Book(string title, DateTime datePublished, Guid authorId) : base()
        {
            Title = title;
            DatePublished = datePublished;
            AuthorId = authorId;
        }
    }
}