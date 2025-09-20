using Domain;
using Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class BookWriteWriteRepository : WriteWriteRepository<Book>, IBookWriteRepository
{
    public BookWriteWriteRepository(CatalogDbContext dbContext) : base(dbContext)
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