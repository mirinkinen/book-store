using Domain.Orders;
using MediatR;

namespace Application.OrderCommands.DeleteOrder;

public record DeleteOrderCommand(Guid Id) : IRequest<DeleteOrderPayload>;

public record DeleteOrderPayload(bool Success, Guid Id);

public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, DeleteOrderPayload>
{
    private readonly IOrderWriteRepository _orderWriteRepository;

    public DeleteOrderHandler(IOrderWriteRepository orderWriteRepository)
    {
        _orderWriteRepository = orderWriteRepository;
    }

    public async Task<DeleteOrderPayload> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
    {
        var success = await _orderWriteRepository.DeleteAsync(command.Id, cancellationToken);

        return new DeleteOrderPayload(success, command.Id);
    }
}