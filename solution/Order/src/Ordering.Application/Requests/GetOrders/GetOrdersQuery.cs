using Common.Api.Application.Authentication;
using Ordering.Application.Services;
using Ordering.Domain.Orders;

namespace Ordering.Application.Requests.GetOrders;

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