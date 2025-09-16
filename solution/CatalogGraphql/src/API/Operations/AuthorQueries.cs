using Application.AuthorCommands;
using Application.AuthorQueries.GetAuthor;
using Application.AuthorQueries.GetAuthors;
using Application.Types;
using MediatR;

namespace API.Operations;

[ExtendObjectType(OperationTypeNames.Query)]
public class AuthorQueries
{
    public async Task<AuthorDto?> GetAuthorById(Guid id, IMediator mediator)
    {
        return await mediator.Send(new GetAuthorByIdQuery(id));
    }

    public async Task<IQueryable<AuthorDto>> GetAuthors(IMediator mediator)
    {
        return await mediator.Send(new GetAuthorsQuery());
    }
}
