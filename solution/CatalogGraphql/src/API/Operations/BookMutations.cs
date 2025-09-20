using Application.BookCommands.CreateBook;
using Application.BookCommands.DeleteBook;
using Application.BookCommands.UpdateBook;
using Application.Types;
using Common.Domain;
using MediatR;

namespace API.Operations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class BookMutations
{
    [Error<DomainRuleException>]
    public async Task<BookDto> CreateBook(Guid authorId, string title, DateOnly datePublished, decimal price, IMediator mediator)
    {
        return await mediator.Send(new CreateBookCommand(authorId, title, datePublished, price));
    }

    [Error<DomainRuleException>]
    [Error<EntityNotFoundException>]
    public async Task<BookDto> UpdateBook(Guid id, string title, DateOnly datePublished, decimal price, IMediator mediator)
    {
        return await mediator.Send(new UpdateBookCommand(id, title, datePublished, price));
    }
    
    [Error<EntityNotFoundException>]
    public async Task<DeleteBookPayload> DeleteBook(Guid id, IMediator mediator)
    {
        return await mediator.Send(new DeleteBookCommand(id));
    }
}