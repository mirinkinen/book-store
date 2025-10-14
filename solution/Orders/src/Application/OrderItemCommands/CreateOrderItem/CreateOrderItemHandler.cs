using Application.OrderItemQueries;
using Domain.Orders;
using Domain.OrderItems;
using MediatR;

namespace Application.OrderItemCommands.CreateOrderItem;

public record CreateOrderItemCommand(
    Guid OrderId,
    string ProductName,
    int Quantity,
    decimal UnitPrice) : IRequest<OrderItemNode>;

public class CreateOrderItemHandler : IRequestHandler<CreateOrderItemCommand, OrderItemNode>
{
    private readonly IOrderWriteRepository _orderWriteRepository;
    private readonly IOrderItemWriteRepository _orderItemWriteRepository;

    public CreateOrderItemHandler(IOrderWriteRepository orderWriteRepository, IOrderItemWriteRepository orderItemWriteRepository)
    {
        _orderWriteRepository = orderWriteRepository;
        _orderItemWriteRepository = orderItemWriteRepository;
    }

    public async Task<OrderItemNode> Handle(CreateOrderItemCommand command, CancellationToken cancellationToken)
    {
        var order = await _orderWriteRepository.FirstOrDefaultAsync(command.OrderId);
        if (order == null)
        {
            throw new ArgumentException($"Order with ID {command.OrderId} not found");
        }

        var orderItem = new OrderItem(command.OrderId, command.ProductName, command.Quantity, command.UnitPrice);
        orderItem.SetOrder(order);

        _orderItemWriteRepository.Add(orderItem);
        await _orderItemWriteRepository.SaveChangesAsync(cancellationToken);

        return orderItem.MapToDto();
    }
}