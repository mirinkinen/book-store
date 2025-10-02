using Application.AuthorQueries;
using Application.BookQueries;
using Infra.DataLoaders;

namespace API.BookOperations;

[ObjectType<BookNode>]
public static partial class BookType
{
    // public static async Task<AuthorNode?> GetAuthorAsync([Parent] BookNode book, IAuthorByBookIdDataLoader dataLoader)
    // {
    //     var author = await dataLoader.LoadAsync(book.Id);
    //     return author;
    // }
}