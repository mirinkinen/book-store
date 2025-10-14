using Application.OrderQueries;
using Domain.Orders;
using MediatR;

namespace Application.OrderCommands.UpdateOrder;

public record UpdateOrderCommand(
    Guid Id,
    string CustomerName,
    DateOnly OrderDate) : IRequest<OrderNode>;

public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, OrderNode>
{
    private readonly IOrderWriteRepository _orderWriteRepository;

    public UpdateOrderHandler(IOrderWriteRepository orderWriteRepository)
    {
        _orderWriteRepository = orderWriteRepository;
    }

    public async Task<OrderNode> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = await _orderWriteRepository.FirstOrDefaultAsync(command.Id);
        if (order == null)
        {
            throw new ArgumentException($"Order with ID {command.Id} not found");
        }

        order.Update(command.CustomerName, command.OrderDate);

        await _orderWriteRepository.SaveChangesAsync(cancellationToken);

        return order.MapToDto();
    }
}