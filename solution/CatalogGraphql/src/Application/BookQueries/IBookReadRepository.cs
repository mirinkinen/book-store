using Application.Services;
using Domain;
using Domain.Books;

namespace Application.BookQueries;

public interface IBookReadRepository : IReadRepository<Book, BookNode>
{
}