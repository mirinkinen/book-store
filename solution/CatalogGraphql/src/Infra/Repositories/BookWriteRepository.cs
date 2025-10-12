using Domain;
using Domain.Books;
using Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class BookWriteRepository : WriteRepository<Book>, IBookWriteRepository
{
    public BookWriteRepository(CatalogDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Book>> GetByAuthorIdAsync(Guid authorId)
    {
        return await DbContext.Books
            .Include(b => b.Author)
            .Where(b => b.AuthorId == authorId)
            .ToListAsync();
    }
}