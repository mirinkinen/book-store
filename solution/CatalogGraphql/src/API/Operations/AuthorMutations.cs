using Application.AuthorMutations.CreateAuthor;
using Application.AuthorMutations.DeleteAuthor;
using Application.AuthorMutations.UpdateAuthor;
using MediatR;

namespace API.Operations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class AuthorMutations
{
    public async Task<AuthorCreatedOutput> CreateAuthor(CreateAuthorInput input, IMediator mediator)
    {
        return await mediator.Send(input);
    }

    public async Task<AuthorUpdatedOutput> UpdateAuthor(UpdateAuthorInput input, IMediator mediator)
    {
        return await mediator.Send(input);
    }

    public async Task<DeleteAuthorOutput> DeleteAuthor(DeleteAuthorInput input, IMediator mediator)
    {
        return await mediator.Send(input);
    }
}