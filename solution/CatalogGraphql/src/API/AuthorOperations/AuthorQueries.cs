using Application.AuthorQueries;
using Application.AuthorQueries.GetAuthorById;
using Application.AuthorQueries.GetAuthors;
using Common.Domain;
using GreenDonut.Data;
using HotChocolate.Types.Pagination;
using MediatR;

namespace API.AuthorOperations;

[QueryType]
public static partial class AuthorQueries
{
    [NodeResolver]
    [Error<EntityNotFoundException>]
    public static async Task<AuthorDto> GetAuthorById(Guid id, ISender sender)
    {
        return await sender.Send(new GetAuthorByIdQuery(id));
    }

    [UseConnection]
    [UseFiltering]
    [UseSorting]
    public static async Task<PageConnection<AuthorDto>> GetAuthors(
        PagingArguments pagingArguments,
        QueryContext<AuthorDto> queryContext,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var page = await sender.Send(new GetAuthorsQuery(pagingArguments, queryContext), cancellationToken);
        return new PageConnection<AuthorDto>(page);
    }
}