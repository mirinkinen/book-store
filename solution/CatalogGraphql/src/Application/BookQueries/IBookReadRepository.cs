using Application.Common;
using Domain;

namespace Application.BookQueries;

public interface IBookReadRepository : IReadRepository<Book, BookNode>
{
}