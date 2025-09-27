using Application.AuthorQueries;
using Application.BookQueries;
using GreenDonut;
using Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Infra.DataLoaders;

public static class DataLoaders
{
    [DataLoader]
    internal static async Task<ILookup<Guid, BookDto>> GetBooksByAuthorIdsAsync(
        IReadOnlyList<Guid> authorIds,
        CatalogDbContext dbContext,
        CancellationToken cancellationToken)
    {
        return (await dbContext.Books
                .Where(b => authorIds.Contains(b.AuthorId))
                .Select(b => b.ToDto())
                .ToListAsync(cancellationToken))
            .ToLookup(b => b.AuthorId);
    }
    
    [DataLoader]
    internal static async Task<Dictionary<Guid, AuthorDto>> GetAuthorByIdAsync(
        IReadOnlyList<Guid> authorIds,
        CatalogDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var authors = await dbContext.Authors
            .Where(a => authorIds.Contains(a.Id))
            .Select(a => a.ToDto())
            .Distinct()
            .ToDictionaryAsync(a => a.Id, cancellationToken);
        
        return authors;
    }
}