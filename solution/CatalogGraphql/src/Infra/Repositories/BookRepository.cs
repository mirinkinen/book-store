using Application.Repositories;
using Domain;
using Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class BookRepository : IBookRepository
{
    private readonly IDbContextFactory<CatalogDbContext> _contextFactory;

    public BookRepository(IDbContextFactory<CatalogDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<Book?> GetByIdAsync(Guid id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Books
            .Include(b => b.Author)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Books
            .Include(b => b.Author)
            .ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetByAuthorIdAsync(Guid authorId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Books
            .Include(b => b.Author)
            .Where(b => b.AuthorId == authorId)
            .ToListAsync();
    }

    public async Task<Book> AddAsync(Book book)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        context.Books.Add(book);
        await context.SaveChangesAsync();
        return book;
    }

    public async Task<Book> UpdateAsync(Book book)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        context.Entry(book).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return book;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var book = await context.Books.FindAsync(id);
        if (book == null)
            return false;

        context.Books.Remove(book);
        await context.SaveChangesAsync();
        return true;
    }
}
