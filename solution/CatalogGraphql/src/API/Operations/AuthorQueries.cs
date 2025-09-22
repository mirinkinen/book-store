using Application.AuthorQueries.GetAuthorById;
using Application.AuthorQueries.GetAuthors;
using Application.Types;
using Common.Domain;
using Domain;
using GreenDonut.Data;
using MediatR;

namespace API.Operations;

[QueryType]
public class AuthorQueries
{
    [NodeResolver]
    [Error<EntityNotFoundException>]
    public async Task<Author> GetAuthorById(Guid id, ISender sender)
    {
        return await sender.Send(new GetAuthorByIdQuery(id));
    }

    [UsePaging(MaxPageSize = 10, DefaultPageSize = 10, IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<Author>> GetAuthors(QueryContext<Author> queryContext, ISender sender)
    {
        return await sender.Send(new GetAuthorsQuery(queryContext));
    }
}