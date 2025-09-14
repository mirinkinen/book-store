using Application.AuthorQueries.GetAuthor;
using Application.AuthorQueries.GetAuthors;
using MediatR;

namespace API.Operations;

[ExtendObjectType(OperationTypeNames.Query)]
public class AuthorQueries
{
    public async Task<GetAuthorOutput?> GetAuthorById(Guid id, IMediator mediator)
    {
        return await mediator.Send(new GetAuthorByIdInput(id));
    }

    public async Task<IEnumerable<GetAuthorOutput>> GetAuthors(IMediator mediator)
    {
        return await mediator.Send(new GetAuthorsInput());
    }
}
