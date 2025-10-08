using Application.AuthorQueries;
using Application.AuthorQueries.GetAuthorById;
using Application.AuthorQueries.GetAuthors;
using Application.Services;
using Common.Domain;
using GreenDonut.Data;
using HotChocolate.Types.Pagination;
using MediatR;

namespace API.AuthorOperations;

[QueryType]
public static partial class AuthorQueries
{
    /// <summary>
    /// Gets author by ID.
    /// </summary>
    /// <param name="id">The ID of the author</param>
    /// <returns>Author</returns>
    [NodeResolver]
    [Error<EntityNotFoundException>]
    public static async Task<AuthorNode> GetAuthorById(Guid id, ISender sender)
    {
        return await sender.Send(new GetAuthorByIdQuery(id));
    }

    /// <summary>
    /// Gets authors by query parameters.
    /// </summary>
    /// <returns>List of authors</returns>
    [UseConnection]
    [UseFiltering]
    [UseSorting]
    public static async Task<PageConnection<AuthorNode>> GetAuthors(
        PagingArguments pagingArguments,
        QueryContext<AuthorNode> queryContext,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var page = await sender.Send(new GetAuthorsQuery(pagingArguments, queryContext), cancellationToken);
        return new PageConnection<AuthorNode>(page);
    }

    /// <summary>
    /// Demonstrates how queries are executed in parallel.
    /// </summary>
    public static Task<string> ConcurrentQuery(ScopedService scopedService)
    {
        return scopedService.GetValue();
    }
}