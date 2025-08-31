using Application.Repositories;
using Domain;

namespace API.Operations;

[ExtendObjectType(OperationTypeNames.Query)]
public class AuthorQueries
{
    public async Task<Author?> GetAuthor(Guid id, IAuthorRepository authorRepository)
    {
        return await authorRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Author>> GetAuthors(IAuthorRepository authorRepository)
    {
        return await authorRepository.GetAllAsync();
    }
}
