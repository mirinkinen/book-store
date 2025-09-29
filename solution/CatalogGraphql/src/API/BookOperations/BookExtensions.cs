using Application.AuthorQueries;
using Application.BookQueries;
using Infra.DataLoaders;

namespace API.BookOperations;

[ExtendObjectType<BookDto>]
public class BookExtensions
{
    public async Task<AuthorDto?> GetAuthor([Parent] BookDto book, AuthorByBookIdDataLoader dataLoader)
    {
        var author = await dataLoader.LoadAsync(book.Id);
        return author;
    }
}