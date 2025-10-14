using Application.OrderQueries;
using Application.OrderItemQueries;
using GreenDonut.Data;
using HotChocolate.Types.Pagination;
using Infra.DataLoaders;

namespace API.OrderOperations;

[ObjectType<OrderNode>]
public static partial class OrderType
{
    [UseFiltering]
    [UseSorting]
    public static async Task<PageConnection<OrderItemNode>> GetOrderItemsAsync(
        [Parent] OrderNode order,
        PagingArguments pagingArguments,
        QueryContext<OrderItemNode> query,
        IOrderItemsByOrderIdDataLoader dataLoader,
        CancellationToken cancellationToken)
    {
        var page = await dataLoader.With(pagingArguments, query).LoadAsync(order.Id, cancellationToken);

        return new PageConnection<OrderItemNode>(page ?? Page<OrderItemNode>.Empty);
    }
}