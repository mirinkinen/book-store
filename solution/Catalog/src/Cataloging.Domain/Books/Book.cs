using Cataloging.Domain.Authors;
using Cataloging.Domain.SeedWork;

namespace Cataloging.Domain.Books
{
    public class Book : Entity
    {
        public string Title { get; private set; }

        public DateTime DatePublished { get; private set; }

        public Author Author { get; private set; }

        public Guid AuthorId { get; private set; }

        public Book(string title, DateTime datePublished, Guid authorId)
        {
            Title = title;
            DatePublished = datePublished;
            AuthorId = authorId;
        }
    }
}