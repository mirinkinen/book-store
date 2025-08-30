using Application.Repositories;
using Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Types;

[QueryType]
public class Query
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;

    public Query(IBookRepository bookRepository, IAuthorRepository authorRepository)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
    }

    public async Task<Book?> GetBook(Guid id)
    {
        return await _bookRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Book>> GetBooks()
    {
        return await _bookRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Book>> GetBooksByAuthor(Guid authorId)
    {
        return await _bookRepository.GetByAuthorIdAsync(authorId);
    }

    public async Task<Author?> GetAuthor(Guid id)
    {
        return await _authorRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Author>> GetAuthors()
    {
        return await _authorRepository.GetAllAsync();
    }
}