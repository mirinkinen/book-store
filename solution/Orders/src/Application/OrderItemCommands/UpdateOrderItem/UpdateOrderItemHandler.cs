using Application.OrderItemQueries;
using Domain.OrderItems;
using MediatR;

namespace Application.OrderItemCommands.UpdateOrderItem;

public record UpdateOrderItemCommand(
    Guid Id,
    string ProductName,
    int Quantity,
    decimal UnitPrice) : IRequest<OrderItemNode>;

public class UpdateOrderItemHandler : IRequestHandler<UpdateOrderItemCommand, OrderItemNode>
{
    private readonly IOrderItemWriteRepository _orderItemWriteRepository;

    public UpdateOrderItemHandler(IOrderItemWriteRepository orderItemWriteRepository)
    {
        _orderItemWriteRepository = orderItemWriteRepository;
    }

    public async Task<OrderItemNode> Handle(UpdateOrderItemCommand command, CancellationToken cancellationToken)
    {
        var orderItem = await _orderItemWriteRepository.FirstOrDefaultAsync(command.Id);
        if (orderItem == null)
        {
            throw new ArgumentException($"OrderItem with ID {command.Id} not found");
        }

        orderItem.ProductName = command.ProductName;
        orderItem.Quantity = command.Quantity;
        orderItem.UnitPrice = command.UnitPrice;

        await _orderItemWriteRepository.SaveChangesAsync(cancellationToken);

        return orderItem.MapToDto();
    }
}