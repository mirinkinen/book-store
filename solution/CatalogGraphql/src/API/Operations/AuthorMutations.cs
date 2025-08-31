using Application.Repositories;
using Domain;

namespace API.Operations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class AuthorMutations
{
    public async Task<Author> CreateAuthor(Author input, IAuthorRepository authorRepository)
    {
        var author = new Author(input.FirstName, input.LastName, input.Birthdate, input.OrganizationId);
        var createdAuthor = await authorRepository.AddAsync(author);
        return createdAuthor;
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
