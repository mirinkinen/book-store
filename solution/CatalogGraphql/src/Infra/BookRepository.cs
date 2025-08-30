using Application.Repositories;
using Domain;

public class BookRepository : IBookRepository
{
    public Task<Book?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Book>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Book>> GetByAuthorIdAsync(Guid authorId)
    {
        throw new NotImplementedException();
    }

    public Task<Book> AddAsync(Book book)
    {
        throw new NotImplementedException();
    }

    public Task<Book> UpdateAsync(Book book)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}