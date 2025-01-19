using Common.Application.Authentication;
using Ordering.Application;
using Ordering.Requests.Orders.Domain.Orders;

namespace Ordering.Requests.Orders.Application.GetOrderById;

public record GetOrderByIdQuery(Guid OrderId);

public class GetOrderByIdHandler
{
    private readonly IQueryAuthorizer _queryAuthorizer;

    public GetOrderByIdHandler(IQueryAuthorizer queryAuthorizer)
    {
        _queryAuthorizer = queryAuthorizer;
    }

    public Task<IQueryable<Order>> Handle(GetOrderByIdQuery request)
    {
        ArgumentNullException.ThrowIfNull(request);

        return Task.FromResult(_queryAuthorizer.GetAuthorizedEntities<Order>().Where(a => a.Id == request.OrderId));
    }
}