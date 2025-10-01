using Application.AuthorQueries;
using Application.BookQueries;
using GreenDonut;
using GreenDonut.Data;
using Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Infra.DataLoaders;

public static class DataLoaders
{
    [DataLoader]
    internal static async Task<Dictionary<Guid, Page<BookNode>>> GetBooksByAuthorIdsAsync(
        IReadOnlyList<Guid> authorIds,
        PagingArguments pagingArguments,
        CatalogDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var page = await dbContext.Books
                .Where(b => authorIds.Contains(b.AuthorId))
                .Select(b => new BookNode
                {
                    Id = b.Id,
                    Title = b.Title,
                    Price = b.Price,
                    AuthorId = b.AuthorId,
                    DatePublished = b.DatePublished
                })
                .OrderBy(b => b.Title)
                .ThenBy(b => b.Id)
                .ToBatchPageAsync(b => b.AuthorId, pagingArguments, cancellationToken);

        return page;
    }
    
    [DataLoader]
    internal static async Task<Dictionary<Guid, AuthorNode>> GetAuthorByIdAsync(
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
    
    [DataLoader]
    internal static async Task<Dictionary<Guid, AuthorNode>> GetAuthorByBookIdAsync(
        IReadOnlyList<Guid> bookIds,
        CatalogDbContext dbContext,
        CancellationToken cancellationToken)
    {
        return await dbContext.Books
            .Where(b => bookIds.Contains(b.Id))
            .Join(dbContext.Authors, 
                  book => book.AuthorId, 
                  author => author.Id, 
                  (book, author) => new { BookId = book.Id, AuthorId = book.AuthorId, Author = author.ToDto() })
            .OrderBy(x => x.BookId)
            .ToDictionaryAsync(x => x.BookId, x => x.Author, cancellationToken);
    }
}