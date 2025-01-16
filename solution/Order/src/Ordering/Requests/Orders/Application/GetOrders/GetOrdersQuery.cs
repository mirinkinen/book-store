using Common.Application.Authentication;
using Ordering.Application;
using Ordering.Requests.Orders.Domain.Orders;

namespace Ordering.Requests.Orders.Application.GetOrders;

public record GetOrdersQuery(User Actor);

public class GetOrdersHandler
{
    private readonly IQueryAuthorizer _queryAuthorizer;

    public GetOrdersHandler(IQueryAuthorizer queryAuthorizer)
    {
        _queryAuthorizer = queryAuthorizer;
    }

    public Task<IQueryable<Order>> Handle(GetOrdersQuery request)
    {
        return Task.FromResult(_queryAuthorizer.GetAuthorizedEntities<Order>());
    }
}