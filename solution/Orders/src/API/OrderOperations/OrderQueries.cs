using Application.OrderQueries;
using Application.OrderQueries.GetOrderById;
using Application.OrderQueries.GetOrders;
using Common.Domain;
using GreenDonut.Data;
using HotChocolate.Types.Pagination;
using MediatR;

namespace API.OrderOperations;

[QueryType]
public static partial class OrderQueries
{
    /// <summary>
    /// Gets order by ID.
    /// </summary>
    /// <param name="id">The ID of the order</param>
    /// <returns>Order</returns>
    [NodeResolver]
    [Error<EntityNotFoundException>]
    public static async Task<OrderNode> GetOrderById(Guid id, ISender sender)
    {
        return await sender.Send(new GetOrderByIdQuery(id));
    }

    /// <summary>
    /// Gets orders by query parameters.
    /// </summary>
    /// <returns>List of orders</returns>
    [UseConnection]
    [UseFiltering]
    [UseSorting]
    public static async Task<PageConnection<OrderNode>> GetOrders(
        PagingArguments pagingArguments,
        QueryContext<OrderNode> queryContext,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var page = await sender.Send(new GetOrdersQuery(pagingArguments, queryContext), cancellationToken);
        return new PageConnection<OrderNode>(page);
    }
}