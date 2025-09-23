using Application.AuthorQueries.GetAuthorById;
using Application.AuthorQueries.GetAuthors;
using Common.Domain;
using Domain;
using GreenDonut.Data;
using HotChocolate.Types.Pagination;
using MediatR;

namespace API.Operations;

[QueryType]
public static partial class AuthorQueries
{
    [NodeResolver]
    [Error<EntityNotFoundException>]
    public static async Task<Author> GetAuthorById(Guid id, ISender sender)
    {
        return await sender.Send(new GetAuthorByIdQuery(id));
    }

    [UseConnection]
    [UseFiltering]
    [UseSorting]
    public static async Task<PageConnection<Author>> GetAuthors(
        PagingArguments pagingArguments, 
        QueryContext<Author> queryContext, 
        ISender sender, 
        CancellationToken cancellationToken)
    {
        var page = await sender.Send(new GetAuthorsQuery(pagingArguments, queryContext), cancellationToken);
        return new PageConnection<Author>(page);
    }
}