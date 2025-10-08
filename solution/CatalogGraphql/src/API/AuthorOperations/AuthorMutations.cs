using Application.AuthorCommands.CreateAuthor;
using Application.AuthorCommands.DeleteAuthor;
using Application.AuthorCommands.UpdateAuthor;
using Application.AuthorQueries;
using Application.Services;
using Common.Domain;
using MediatR;

namespace API.AuthorOperations;

[MutationType]
public class AuthorMutations
{
    [Error<DomainRuleException>]
    public async Task<AuthorNode> CreateAuthor(string firstName, string lastName, DateOnly birthdate, Guid organizationId,
        ISender sender, CancellationToken cancellationToken)
    {
        var author = await sender.Send(new CreateAuthorCommand(firstName, lastName, birthdate, organizationId), cancellationToken);
        return author;
    }

    [Error<DomainRuleException>]
    [Error<EntityNotFoundException>]
    public async Task<AuthorNode> UpdateAuthor(Guid id, string firstName, string lastName, DateOnly birthdate, ISender sender)
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
    /// Demonstrates how mutations are executed in sequence.
    /// </summary>
    public Task<string> ConcurrentMutation(ScopedService scopedService)
    {
        return scopedService.GetValue();
    }
}