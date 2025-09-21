using Domain;
using Infra.DataLoaders;

namespace API.Operations;

[ExtendObjectType<Author>]
public class AuthorExtensions
{
    public async Task<IEnumerable<Book>?> GetBooks([Parent] Author author, BooksByAuthorIdDataLoader dataLoader)
    {
        return await dataLoader.LoadAsync(author.Id);
    }
}