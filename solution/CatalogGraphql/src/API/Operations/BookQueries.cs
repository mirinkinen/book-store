using Application.Repositories;
using Domain;

namespace API.Operations;

[ExtendObjectType(OperationTypeNames.Query)]
public class BookQueries
{
    public async Task<Book?> GetBook(Guid id, IBookRepository bookRepository)
    {
        return await bookRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Book>> GetBooks(IBookRepository bookRepository)
    {
        return await bookRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Book>> GetBooksByAuthor(Guid authorId, IBookRepository bookRepository)
    {
        return await bookRepository.GetByAuthorIdAsync(authorId);
    }

}