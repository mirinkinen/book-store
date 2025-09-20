using Application.AuthorCommands.CreateAuthor;
using Application.AuthorCommands.DeleteAuthor;
using Application.AuthorCommands.MediatorHandlerWithMultipleRepositories;
using Application.AuthorCommands.UpdateAuthor;
using Application.AuthorQueries.GetAuthors;
using Application.Types;
using Common.Domain;
using HotChocolate.Subscriptions;
using MediatR;

namespace API.Operations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class AuthorMutations
{
    [Error<DomainRuleException>]
    public async Task<AuthorDto> CreateAuthor(string firstName, string lastName, DateTime birthdate, Guid organizationId, 
        IMediator mediator, CancellationToken cancellationToken)
    {
        var author = await mediator.Send(new CreateAuthorCommand(firstName, lastName, birthdate, organizationId), cancellationToken);
        return author;
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

    /// <summary>
    /// Example that shows that scoped services are shared between concurrent mutations.
    /// </summary>
    public Task<string> MutationTest(ScopedService scopedService)
    {
        return scopedService.GetHelloWorld();
    }
    
    /// <summary>
    /// Example that shows that scoped services are shared between concurrent mutations.
    /// </summary>
    public Task<string> AnotherMutationTest(ScopedService scopedService)
    {
        return scopedService.GetHelloWorld();
    }

    public Task<string> MutationWithMultipleRepositories(IMediator mediator)
    {
        return mediator.Send(new MutationWithMultipleRepositoriesCommand());
    }
}