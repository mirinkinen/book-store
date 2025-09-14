using Application.BookMutations.CreateBook;
using Application.BookMutations.DeleteBook;
using Application.BookMutations.UpdateBook;
using MediatR;

namespace API.Operations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class BookMutations
{
    public async Task<BookCreatedOutput> CreateBook(CreateBookInput input, IMediator mediator)
    {
        return await mediator.Send(input);
    }

    public async Task<BookUpdatedOutput> UpdateBook(UpdateBookInput input, IMediator mediator)
    {
        return await mediator.Send(input);
    }

    public async Task<DeleteBookOutput> DeleteBook(DeleteBookInput input, IMediator mediator)
    {
        return await mediator.Send(input);
    }
}