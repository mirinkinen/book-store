using Application.OrderQueries;
using Application.OrderItemQueries;
using GreenDonut;
using GreenDonut.Data;
using Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace Infra.DataLoaders;

public static class DataLoaders
{
    [DataLoader]
    public static async Task<Dictionary<Guid, Page<OrderItemNode>>> GetOrderItemsByOrderIdAsync(
        IReadOnlyList<Guid> orderIds,
        PagingArguments pagingArgs,
        QueryContext<OrderItemNode> query,
        OrdersDbContext context,
        CancellationToken cancellationToken)
    {
        return await context.OrderItems
            .Where(oi => orderIds.Contains(oi.OrderId))
            .Select(oi => new OrderItemNode
            {
                Id = oi.Id,
                ProductName = oi.ProductName,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice,
                OrderId = oi.OrderId
            })
            .With(query, sort => sort.IfEmpty(s => s.AddAscending(oi => oi.ProductName)).AddAscending(oi => oi.Id))
            .ToBatchPageAsync(oi => oi.OrderId, pagingArgs, cancellationToken);
    }

    [DataLoader]
    internal static async Task<Dictionary<Guid, OrderNode>> GetOrderByIdAsync(
        IReadOnlyList<Guid> orderIds,
        OrdersDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders
            .Where(o => orderIds.Contains(o.Id))
            .Select(OrderExtensions.ProjectToNode())
            .Distinct()
            .ToDictionaryAsync(o => o.Id, cancellationToken);

        return orders;
    }

    [DataLoader]
    internal static async Task<Dictionary<Guid, OrderNode>> GetOrderByOrderItemIdAsync(
        IReadOnlyList<Guid> orderItemIds,
        QueryContext<OrderNode> queryContext,
        OrdersDbContext dbContext,
        CancellationToken cancellationToken)
    {
        // ID is required for the join.
        queryContext = queryContext.Include(order => order.Id);
        
        return await dbContext.OrderItems
            .Where(oi => orderItemIds.Contains(oi.Id))
            .Join(dbContext.Orders
                    .Select(OrderExtensions.ProjectToNode())
                    .With(queryContext),
                orderItem => orderItem.OrderId,
                order => order.Id,
                (orderItem, order) => new { OrderItemId = orderItem.Id, Order = order })
            .ToDictionaryAsync(x => x.OrderItemId, x => x.Order, cancellationToken);
    }
}