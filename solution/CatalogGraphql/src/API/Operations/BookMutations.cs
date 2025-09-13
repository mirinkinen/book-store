using Application.Repositories;
using Common.Domain;
using Domain;

namespace API.Operations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class BookMutations
{
    public async Task<Book> CreateBook(Book input, IAuthorRepository authorRepository, IBookRepository bookRepository)
    {
        var author = await authorRepository.GetByIdAsync(input.AuthorId);
        if (author == null)
        {
            throw new ArgumentException($"Author with ID {input.AuthorId} not found");
        }

        var book = new Book(input.AuthorId, input.Title, input.DatePublished, input.Price);
        book.SetAuthor(author);
        
        var createdBook = await bookRepository.AddAsync(book);
        return createdBook;
    }

    public async Task<Book> UpdateBook(Guid id, string title, DateTime datePublished, decimal price, IBookRepository bookRepository)
    {
        var book = await bookRepository.GetByIdAsync(id);
        if (book == null)
        {
            throw new ArgumentException($"Book with ID {id} not found");
        }

        book.Title = title;
        book.DatePublished = datePublished;
        book.Price = price;

        var updatedBook = await bookRepository.UpdateAsync(book);
        return updatedBook;
    }

    public async Task<bool> DeleteBook(Guid id, IBookRepository bookRepository)
    {
        return await bookRepository.DeleteAsync(id);
    }
}