using Application.Repositories;
using Domain;
using System;
using System.Threading.Tasks;

namespace API.Types;

[MutationType]
public class Mutation
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;

    public Mutation(IBookRepository bookRepository, IAuthorRepository authorRepository)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
    }

    // Author Mutations
    public async Task<Author> CreateAuthor(string firstName, string lastName, DateTime birthdate, Guid organizationId)
    {
        var author = new Author(firstName, lastName, birthdate, organizationId);
        return await _authorRepository.AddAsync(author);
    }

    public async Task<Author> UpdateAuthor(Guid id, string firstName, string lastName, DateTime birthdate)
    {
        var author = await _authorRepository.GetByIdAsync(id);
        if (author == null)
        {
            throw new ArgumentException($"Author with ID {id} not found");
        }
        
        author.Update(firstName, lastName, birthdate);
        return await _authorRepository.UpdateAsync(author);
    }

    public async Task<bool> DeleteAuthor(Guid id)
    {
        return await _authorRepository.DeleteAsync(id);
    }

    // Book Mutations
    public async Task<Book> CreateBook(Guid authorId, string title, DateTime datePublished, decimal price)
    {
        var author = await _authorRepository.GetByIdAsync(authorId);
        if (author == null)
        {
            throw new ArgumentException($"Author with ID {authorId} not found");
        }

        var book = new Book(authorId, title, datePublished, price);
        book.SetAuthor(author);
        
        return await _bookRepository.AddAsync(book);
    }

    public async Task<Book> UpdateBook(Guid id, string title, DateTime datePublished, decimal price)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null)
        {
            throw new ArgumentException($"Book with ID {id} not found");
        }

        book.Title = title;
        book.DatePublished = datePublished;
        book.Price = price;

        return await _bookRepository.UpdateAsync(book);
    }

    public async Task<bool> DeleteBook(Guid id)
    {
        return await _bookRepository.DeleteAsync(id);
    }
}
