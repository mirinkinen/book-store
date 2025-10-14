using Application.OrderQueries;
using Common.Domain;
using Domain.Orders;
using HotChocolate.Subscriptions;
using MediatR;

namespace Application.OrderCommands.CreateOrder;

public record CreateOrderCommand(
    string CustomerName,
    DateOnly OrderDate,
    Guid OrganizationId) : IRequest<OrderNode>;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, OrderNode>
{
    private readonly IOrderWriteRepository _orderWriteRepository;
    private readonly ITopicEventSender _eventSender;

    public CreateOrderHandler(IOrderWriteRepository orderWriteRepository, ITopicEventSender eventSender)
    {
        _orderWriteRepository = orderWriteRepository;
        _eventSender = eventSender;
    }

    public async Task<OrderNode> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = new Order(command.CustomerName, command.OrderDate, command.OrganizationId);
        _orderWriteRepository.Add(order);
        await _orderWriteRepository.SaveChangesAsync(cancellationToken);

        await _eventSender.SendAsync(nameof(CreateOrder), order, cancellationToken);

        return order.MapToDto();
    }
}