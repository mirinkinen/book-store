using Application.AuthorCommands;
using Application.AuthorQueries.GetAuthor;
using Application.AuthorQueries.GetAuthors;
using MediatR;

namespace API.Operations;

[ExtendObjectType(OperationTypeNames.Query)]
public class AuthorQueries
{
    public async Task<AuthorOutputType?> GetAuthorById(Guid id, IMediator mediator)
    {
        return await mediator.Send(new GetAuthorByIdQuery(id));
    }

    public async Task<IEnumerable<AuthorOutputType>> GetAuthors(IMediator mediator)
    {
        return await mediator.Send(new GetAuthorsQuery());
    }
}
