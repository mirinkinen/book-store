using Domain;
using Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Operations;

[ExtendObjectType<Author>]
public class AuthorExtensions
{
    public async Task<IEnumerable<Book>?> GetBooks(
        [Parent] Author author, 
        BooksByAuthorIdDataLoader dataLoader)
    {
        return await dataLoader.LoadAsync(author.Id);
    }

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
