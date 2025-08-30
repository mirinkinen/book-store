using Domain;

namespace Application.Repositories;

public class AuthorRepository : IAuthorRepository
{
    public Task<Author?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Author>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Author> AddAsync(Author author)
    {
        throw new NotImplementedException();
    }

    public Task<Author> UpdateAsync(Author author)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}