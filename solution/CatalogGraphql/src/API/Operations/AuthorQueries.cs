using Application.AuthorCommands;
using Application.AuthorQueries.GetAuthor;
using Application.AuthorQueries.GetAuthors;
using Application.Types;
using Common.Domain;
using MediatR;

namespace API.Operations;

[ExtendObjectType(OperationTypeNames.Query)]
public class AuthorQueries
{
    [Error<EntityNotFoundException>]
    public async Task<AuthorDto> GetAuthorById(Guid id, IMediator mediator)
    {
        return await mediator.Send(new GetAuthorByIdQuery(id));
    }

    [UsePaging(MaxPageSize = 10)]
    public async Task<IQueryable<AuthorDto>> GetAuthors(IMediator mediator)
    {
        return await mediator.Send(new GetAuthorsQuery());
    }
}
