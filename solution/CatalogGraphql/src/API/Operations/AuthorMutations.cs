using Application.AuthorMutations.CreateAuthor;
using Application.Repositories;
using Domain;
using MediatR;

namespace API.Operations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class AuthorMutations
{
    public async Task<AuthorCreatedOutput> CreateAuthor(CreateAuthorInput input, IMediator mediator)
    {
        return await mediator.Send(input);
    }

    public async Task<Author> UpdateAuthor(Guid id, string firstName, string lastName, DateTime birthdate, IAuthorRepository authorRepository)
    {
        var author = await authorRepository.GetByIdAsync(id);
        if (author == null)
        {
            throw new ArgumentException($"Author with ID {id} not found");
        }
        
        author.Update(firstName, lastName, birthdate);
        var updatedAuthor = await authorRepository.UpdateAsync(author);
        return updatedAuthor;
    }

    public async Task<bool> DeleteAuthor(Guid id, IAuthorRepository authorRepository)
    {
        return await authorRepository.DeleteAsync(id);
    }


}
