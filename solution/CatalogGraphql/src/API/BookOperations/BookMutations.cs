using Application.BookCommands.CreateBook;
using Application.BookCommands.DeleteBook;
using Application.BookCommands.UpdateBook;
using Application.BookQueries;
using Common.Domain;
using MediatR;

namespace API.BookOperations;

[MutationType]
public class BookMutations
{
    [Error<DomainRuleException>]
    public async Task<BookNode> CreateBook(Guid authorId, string title, DateOnly datePublished, decimal price, ISender sender)
    {
        return await sender.Send(new CreateBookCommand(authorId, title, datePublished, price));
    }

    [Error<DomainRuleException>]
    [Error<EntityNotFoundException>]
    public async Task<BookNode> UpdateBook(Guid id, string title, DateOnly datePublished, decimal price, ISender sender)
    {
        return await sender.Send(new UpdateBookCommand(id, title, datePublished, price));
    }
    
    [Error<EntityNotFoundException>]
    public async Task<DeleteBookPayload> DeleteBook(Guid id, ISender sender)
    {
        return await sender.Send(new DeleteBookCommand(id));
    }
}