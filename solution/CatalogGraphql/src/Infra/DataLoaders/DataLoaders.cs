using Domain;
using GreenDonut;
using Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Infra.DataLoaders;

public static class DataLoaders
{
    [DataLoader]
    internal static async Task<ILookup<Guid, Book>> GetBooksByAuthorIdAsync(
        IReadOnlyList<Guid> authorIds,
        CatalogDbContext dbContext,
        CancellationToken cancellationToken)
    {
        return (await dbContext.Books
                .Where(b => authorIds.Contains(b.AuthorId))
                .ToListAsync(cancellationToken))
            .ToLookup(b => b.AuthorId);
    }
}