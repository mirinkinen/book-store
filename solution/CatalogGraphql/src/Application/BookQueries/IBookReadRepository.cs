using Application.Services;
using Domain.Books;

namespace Application.BookQueries;

public interface IBookReadRepository : IReadRepository<Book, BookNode>
{
}