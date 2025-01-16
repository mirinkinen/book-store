using Cataloging.Domain;
using Cataloging.Requests.Authors.Domain;

namespace Cataloging.Requests.Books.Domain
{
    public class Book : Entity
    {
        public string Title { get; private set; }

        public DateTime DatePublished { get; private set; }

        public Author Author { get; private set; }

        public Guid AuthorId { get; private set; }

        public decimal Price { get; private set; }

        public Book(Guid authorId, string title, DateTime datePublished, decimal price)
        {
            AuthorId = authorId;
            Title = title;
            DatePublished = datePublished;
            Price = price;
        }
    }
}