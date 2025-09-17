using Application.AuthorCommands.CreateAuthor;
using Application.AuthorCommands.DeleteAuthor;
using Application.AuthorCommands.UpdateAuthor;
using Application.Types;
using Common.Domain;
using MediatR;

namespace API.Operations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class AuthorMutations
{
    [Error<DomainRuleException>]
    public async Task<AuthorDto> CreateAuthor(string firstName, string lastName, DateTime birthdate, Guid organizationId, 
        IMediator mediator)
    {
        return await mediator.Send(new CreateAuthorCommand(firstName, lastName, birthdate, organizationId));
    }

    [Error<DomainRuleException>]
    [Error<EntityNotFoundException>]
    public async Task<AuthorDto> UpdateAuthor(Guid id, string firstName, string lastName, DateTime birthdate, IMediator mediator)
    {
        return await mediator.Send(new UpdateAuthorCommand(id, firstName, lastName, birthdate));
    }

    [Error<DomainRuleException>]
    [Error<EntityNotFoundException>]
    public async Task<DeleteAuthorPayload> DeleteAuthor(Guid Id, IMediator mediator)
    {
        return await mediator.Send(new DeleteAuthorCommand(Id));
    }
}