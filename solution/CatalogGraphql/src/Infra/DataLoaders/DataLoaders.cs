using Application.AuthorQueries;
using Application.BookQueries;
using Application.ReviewQueries;
using GreenDonut;
using GreenDonut.Data;
using Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace Infra.DataLoaders;

public static class DataLoaders
{
    [DataLoader]
    public static async Task<Dictionary<Guid, Page<BookNode>>> GetBooksByAuthorIdAsync(
        IReadOnlyList<Guid> authorIds,
        PagingArguments pagingArgs,
        QueryContext<BookNode> query,
        CatalogDbContext context,
        CancellationToken cancellationToken)
    {
        return await context.Books
            .Where(b => authorIds.Contains(b.AuthorId))
            .Select(b => new BookNode
            {
                Id = b.Id,
                Title = b.Title,
                Price = b.Price,
                AuthorId = b.AuthorId,
                DatePublished = b.DatePublished
            })
            .With(query, sort => sort.IfEmpty(s => s.AddAscending(b => b.Title)).AddAscending(b => b.Id))
            .ToBatchPageAsync(b => b.AuthorId, pagingArgs, cancellationToken);
    }

    [DataLoader]
    internal static async Task<Dictionary<Guid, AuthorNode>> GetAuthorByIdAsync(
        IReadOnlyList<Guid> authorIds,
        CatalogDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var authors = await dbContext.Authors
            .Where(a => authorIds.Contains(a.Id))
            .Select(AuthorExtensions.ProjectToNode())
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
                (book, author) => new { BookId = book.Id, AuthorId = book.AuthorId, Author = author.MapToDto() })
            .OrderBy(x => x.BookId)
            .ToDictionaryAsync(x => x.BookId, x => x.Author, cancellationToken);
    }

    [DataLoader]
    public static async Task<Dictionary<Guid, Page<ReviewNode>>> GetReviewsByBookIdAsync(
        IReadOnlyList<Guid> bookIds,
        PagingArguments pagingArgs,
        QueryContext<ReviewNode> query,
        CatalogDbContext context,
        CancellationToken cancellationToken)
    {
        return await context.Reviews
            .Where(r => bookIds.Contains(r.BookId))
            .Select(r => new ReviewNode
            {
                Id = r.Id,
                Body = r.Body,
                BookId = r.BookId,
                Title = r.Title
            })
            .With(query, sort => sort.IfEmpty(s => s.AddDescending(r => r.Id)))
            .ToBatchPageAsync(b => b.BookId, pagingArgs, cancellationToken);
    }

    [DataLoader]
    internal static async Task<Dictionary<Guid, BookNode>> GetBookByReviewIdAsync(
        IReadOnlyList<Guid> reviewIds,
        CatalogDbContext dbContext,
        CancellationToken cancellationToken)
    {
        return await dbContext.Reviews
            .Where(r => reviewIds.Contains(r.Id))
            .Join(dbContext.Books,
                review => review.BookId,
                book => book.Id,
                (review, book) => new { ReviewId = review.Id, Book = book.MapToDto() })
            .OrderBy(x => x.ReviewId)
            .ToDictionaryAsync(x => x.ReviewId, x => x.Book, cancellationToken);
    }
}