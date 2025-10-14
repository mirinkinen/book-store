using Application.OrderItemCommands.CreateOrderItem;
using Application.OrderItemCommands.DeleteOrderItem;
using Application.OrderItemCommands.UpdateOrderItem;
using Application.OrderItemQueries;
using Common.Domain;
using MediatR;

namespace API.OrderItemOperations;

[MutationType]
public class OrderItemMutations
{
    [Error<DomainRuleException>]
    public async Task<OrderItemNode> CreateOrderItem(Guid orderId, string productName, int quantity, decimal unitPrice,
        ISender sender, CancellationToken cancellationToken)
    {
        var orderItem = await sender.Send(new CreateOrderItemCommand(orderId, productName, quantity, unitPrice), cancellationToken);
        return orderItem;
    }

    [Error<DomainRuleException>]
    [Error<EntityNotFoundException>]
    public async Task<OrderItemNode> UpdateOrderItem(Guid id, string productName, int quantity, decimal unitPrice, ISender sender)
    {
        return await sender.Send(new UpdateOrderItemCommand(id, productName, quantity, unitPrice));
    }

    [Error<DomainRuleException>]
    [Error<EntityNotFoundException>]
    public async Task<DeleteOrderItemPayload> DeleteOrderItem(Guid Id, ISender sender)
    {
        return await sender.Send(new DeleteOrderItemCommand(Id));
    }
}