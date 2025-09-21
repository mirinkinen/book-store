using Application.AuthorQueries.GetAuthor;
using Application.AuthorQueries.GetAuthors;
using Application.Types;
using Common.Domain;
using MediatR;

namespace API.Operations;

[QueryType]
public class AuthorQueries
{
    [NodeResolver]
    [Error<EntityNotFoundException>]
    public async Task<AuthorDto> GetAuthorById(Guid id, ISender sender)
    {
        return await sender.Send(new GetAuthorByIdQuery(id));
    }

    [UsePaging(MaxPageSize = 10)]
    [UseProjection]
    public async Task<IQueryable<AuthorDto>> GetAuthors(ISender sender)
    {
        return await sender.Send(new GetAuthorsQuery());
    }
}