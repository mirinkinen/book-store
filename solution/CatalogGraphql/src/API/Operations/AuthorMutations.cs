using Application.AuthorCommands.CreateAuthor;
using Application.AuthorCommands.DeleteAuthor;
using Application.AuthorCommands.UpdateAuthor;
using Application.Types;
using MediatR;

namespace API.Operations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class AuthorMutations
{
    public async Task<AuthorOutputType> CreateAuthor(CreateAuthorCommand command, IMediator mediator)
    {
        return await mediator.Send(command);
    }

    public async Task<AuthorOutputType> UpdateAuthor(UpdateAuthorCommand command, IMediator mediator)
    {
        return await mediator.Send(command);
    }

    public async Task<DeleteAuthorOutput> DeleteAuthor(DeleteAuthorCommand command, IMediator mediator)
    {
        return await mediator.Send(command);
    }
}