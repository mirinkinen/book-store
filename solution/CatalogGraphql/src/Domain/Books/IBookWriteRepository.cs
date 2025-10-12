using Domain.Reviews;

namespace Domain.Books;

/// <summary>
/// Repository interface for Book entity
/// </summary>
public interface IBookWriteRepository : IWriteRepository<Book>
{
}