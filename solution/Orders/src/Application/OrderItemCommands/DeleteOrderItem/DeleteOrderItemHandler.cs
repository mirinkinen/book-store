using Domain.OrderItems;
using MediatR;

namespace Application.OrderItemCommands.DeleteOrderItem;

public record DeleteOrderItemCommand(Guid Id) : IRequest<DeleteOrderItemPayload>;

public record DeleteOrderItemPayload(bool Success, Guid Id);

public class DeleteOrderItemHandler : IRequestHandler<DeleteOrderItemCommand, DeleteOrderItemPayload>
{
    private readonly IOrderItemWriteRepository _orderItemWriteRepository;

    public DeleteOrderItemHandler(IOrderItemWriteRepository orderItemWriteRepository)
    {
        _orderItemWriteRepository = orderItemWriteRepository;
    }

    public async Task<DeleteOrderItemPayload> Handle(DeleteOrderItemCommand command, CancellationToken cancellationToken)
    {
        var success = await _orderItemWriteRepository.DeleteAsync(command.Id, cancellationToken);

        return new DeleteOrderItemPayload(success, command.Id);
    }
}