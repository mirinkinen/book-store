using Application.AuthorQueries.GetAuthor;
using Application.AuthorQueries.GetAuthors;
using Application.Types;
using Common.Domain;
using Domain;
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
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<Author>> GetAuthors(ISender sender)
    {
        return await sender.Send(new GetAuthorsQuery());
    }
}