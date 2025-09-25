using Application.AuthorQueries;
using Application.BookQueries;
using Infra.DataLoaders;

namespace API.Operations;

[ExtendObjectType<AuthorDto>]
public class AuthorExtensions
{
    public async Task<IEnumerable<BookDto>?> GetBooks([Parent] AuthorDto author, BooksByAuthorIdDataLoader dataLoader)
    {
        return await dataLoader.LoadAsync(author.Id);
    }
}