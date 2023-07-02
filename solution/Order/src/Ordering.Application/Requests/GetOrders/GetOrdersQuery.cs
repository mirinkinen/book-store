using Ordering.Application.Services;
using Ordering.Domain.Orders;
using Shared.Application.Auditing;
using Shared.Application.Authentication;

namespace Ordering.Application.Requests.GetOrders;

public record GetOrdersQuery(User Actor) : IAuditableQuery;

public class GetOrdersHandler
{
    private readonly IQueryAuthorizer _queryAuthorizer;

    public GetOrdersHandler(IQueryAuthorizer queryAuthorizer)
    {
        _queryAuthorizer = queryAuthorizer;
    }

    public Task<IQueryable<Order>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_queryAuthorizer.GetAuthorizedEntities<Order>());
    }
}