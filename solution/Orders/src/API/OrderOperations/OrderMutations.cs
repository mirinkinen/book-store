using Application.OrderCommands.CreateOrder;
using Application.OrderCommands.DeleteOrder;
using Application.OrderCommands.UpdateOrder;
using Application.OrderQueries;
using Common.Domain;
using MediatR;

namespace API.OrderOperations;

[MutationType]
public class OrderMutations
{
    [Error<DomainRuleException>]
    public async Task<OrderNode> CreateOrder(string customerName, DateOnly orderDate, Guid organizationId,
        ISender sender, CancellationToken cancellationToken)
    {
        var order = await sender.Send(new CreateOrderCommand(customerName, orderDate, organizationId), cancellationToken);
        return order;
    }

    [Error<DomainRuleException>]
    [Error<EntityNotFoundException>]
    public async Task<OrderNode> UpdateOrder(Guid id, string customerName, DateOnly orderDate, ISender sender)
    {
        return await sender.Send(new UpdateOrderCommand(id, customerName, orderDate));
    }

    [Error<DomainRuleException>]
    [Error<EntityNotFoundException>]
    public async Task<DeleteOrderPayload> DeleteOrder(Guid Id, ISender sender)
    {
        return await sender.Send(new DeleteOrderCommand(Id));
    }
}