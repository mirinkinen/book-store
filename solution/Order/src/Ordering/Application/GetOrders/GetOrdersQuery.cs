using Ordering.Domain;

namespace Ordering.Application.GetOrders;

public record GetOrdersQuery();

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