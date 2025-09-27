using Application.AuthorQueries;
using Application.BookQueries;
using Infra.DataLoaders;

namespace API.Operations;

[ExtendObjectType<BookDto>]
public class BookExtensions
{
    public async Task<AuthorDto?> GetAuthor([Parent] BookDto book, AuthorByIdDataLoader dataLoader)
    {
        var author = await dataLoader.LoadAsync(book.AuthorId);
        return author;
    }
}