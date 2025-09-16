using Domain;
using Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class AuthorRepository : IAuthorRepository
{
    private readonly IDbContextFactory<CatalogDbContext> _contextFactory;

    public AuthorRepository(IDbContextFactory<CatalogDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<Author?> GetByIdAsync(Guid id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Authors
            .Include(a => a.Books)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IQueryable<Author>> GetAllAsync()
    {
        var context = await _contextFactory.CreateDbContextAsync();
        return context.Authors.Include(a => a.Books);
    }

    public async Task<Author> AddAsync(Author author)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        context.Authors.Add(author);
        await context.SaveChangesAsync();
        return author;
    }

    public async Task<Author> UpdateAsync(Author author)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        context.Entry(author).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return author;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var author = await context.Authors.FindAsync(id);
        if (author == null)
            return false;

        context.Authors.Remove(author);
        await context.SaveChangesAsync();
        return true;
    }
}
