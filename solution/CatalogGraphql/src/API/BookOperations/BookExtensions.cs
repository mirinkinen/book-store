using Application.AuthorQueries;
using Application.BookQueries;
using Infra.DataLoaders;

namespace API.BookOperations;

[ExtendObjectType<BookNode>]
public class BookExtensions
{
    public async Task<AuthorNode?> GetAuthor([Parent] BookNode book, AuthorByBookIdDataLoader dataLoader)
    {
        var author = await dataLoader.LoadAsync(book.Id);
        return author;
    }
}