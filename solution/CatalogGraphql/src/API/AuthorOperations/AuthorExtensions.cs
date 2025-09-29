using Application.AuthorQueries;
using Application.BookQueries;
using Infra.DataLoaders;

namespace API.AuthorOperations;

[ExtendObjectType<AuthorDto>]
public class AuthorExtensions
{
    // public async Task<IEnumerable<BookDto>?> GetBooks([Parent] AuthorDto author, CustomBooksByAuthorIdsDataLoader dataLoader)
    public async Task<IEnumerable<BookDto>?> GetBooks([Parent] AuthorDto author, BooksByAuthorIdsDataLoader dataLoader)
    {
        var books = await dataLoader.LoadAsync(author.Id);
        return books;
    }
}