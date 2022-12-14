using Books4.Business;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Books4.Data
{
    /// <summary>
    /// This class is declared 'internal' to demonstrate, that it can be hidden to Business layer but visible to WebApi
    /// layer to be able to register it in the DI collection.
    /// </summary>
    internal class BookRepository : IBookRepository
    {
        private readonly List<Author> _authors;
        private readonly List<Book> _books;

        public BookRepository()
        {
            _authors = new List<Author>();
            _books = new List<Book>();

            CreateBooks();
        }

        public void AddBook(Book book)
        {
            book.Id = _books.Max(b => b.Id) + 1; // Give book an ID.
            _books.Add(book);
        }

        public IEnumerable<Book> GetBooks()
        {
            return _books.AsReadOnly();
        }

        private void CreateBooks()
        {
            var bob = new Author
            {
                Id = 1,
                Name = "Robert Cecil Martin",
                DateOfBirth = new DateTime(1952, 12, 5),
            };
            var jane = new Author
            {
                Id = 2,
                Name = "Jane Austen",
                DateOfBirth = new DateTime(1775, 12, 16)
            };

            var cleanArchitecture = new Book
            {
                Id = 1,
                Name = "Clean Architecture: A Craftsman's Guide to Software Structure and Design",
                AuthorId = bob.Id,
                DateOfPublication = new DateTime(2017, 9, 17),
                Genre = Genre.NonFiction
            };

            var senseAndSensibility = new Book
            {
                Id = 2,
                Name = "Sense and Sensibility",
                AuthorId = jane.Id,
                DateOfPublication = new DateTime(1811, 1, 1),
                Genre = Genre.Romance
            };

            bob.Books.Add(cleanArchitecture);
            jane.Books.Add(senseAndSensibility);

            _authors.Add(bob);
            _authors.Add(jane);

            _books.Add(cleanArchitecture);
            _books.Add(senseAndSensibility);
        }
    }
}