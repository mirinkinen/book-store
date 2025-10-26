using Application.OrderItemQueries;
using Application.OrderItemQueries.GetOrderItemById;
using Application.OrderItemQueries.GetOrderItems;
using Common.Domain;
using GreenDonut.Data;
using HotChocolate.Types.Pagination;
using MediatR;

namespace API.OrderItemOperations;

[QueryType]
public static partial class OrderItemQueries
{
    /// <summary>
    /// Gets order item by ID.
    /// </summary>
    /// <param name="id">The ID of the order item</param>
    /// <param name="sender">The mediator sender</param>
    /// <returns>Order item</returns>
    [NodeResolver]
    [Error<EntityNotFoundException>]
    public static async Task<OrderItemNode> GetOrderItemById(Guid id, ISender sender)
    {
        return await sender.Send(new GetOrderItemByIdQuery(id));
    }

    /// <summary>
    /// Gets order items by query parameters.
    /// </summary>
    /// <returns>List of order items</returns>
    [UseConnection]
    [UseFiltering]
    [UseSorting]
    public static async Task<PageConnection<OrderItemNode>> GetOrderItems(
        PagingArguments pagingArguments,
        QueryContext<OrderItemNode> queryContext,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var page = await sender.Send(new GetOrderItemsQuery(pagingArguments, queryContext), cancellationToken);
        return new PageConnection<OrderItemNode>(page);
    }
}