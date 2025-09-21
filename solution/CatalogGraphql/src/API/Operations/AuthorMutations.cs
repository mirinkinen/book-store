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
    public async Task<AuthorDto> CreateAuthor(string firstName, string lastName, DateOnly birthdate, Guid organizationId, 
        ISender sender, CancellationToken cancellationToken)
    {
        var author = await sender.Send(new CreateAuthorCommand(firstName, lastName, birthdate, organizationId), cancellationToken);
        return author;
    }

    [Error<DomainRuleException>]
    [Error<EntityNotFoundException>]
    public async Task<AuthorDto> UpdateAuthor(Guid id, string firstName, string lastName, DateOnly birthdate, ISender sender)
    {
        return await sender.Send(new UpdateAuthorCommand(id, firstName, lastName, birthdate));
    }

    [Error<DomainRuleException>]
    [Error<EntityNotFoundException>]
    public async Task<DeleteAuthorPayload> DeleteAuthor(Guid Id, ISender sender)
    {
        return await sender.Send(new DeleteAuthorCommand(Id));
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

    public Task<string> MutationWithMultipleRepositories(ISender sender)
    {
        return sender.Send(new MutationWithMultipleRepositoriesCommand());
    }
}