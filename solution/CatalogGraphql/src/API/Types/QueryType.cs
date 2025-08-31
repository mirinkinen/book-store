using Application.Repositories;
using Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Types;

[QueryType]
public class Query
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

    public async Task<Author?> GetAuthor(Guid id, IAuthorRepository authorRepository)
    {
        return await authorRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Author>> GetAuthors(IAuthorRepository authorRepository)
    {
        return await authorRepository.GetAllAsync();
    }
}