using Ordering.Application.Services;
using Ordering.Domain.Orders;
using Shared.Application.Authentication;

namespace Ordering.Application.Requests.GetOrderById;

public record GetOrderByIdQuery(Guid OrderId, User Actor);

public class GetOrderByIdHandler
{
    private readonly IQueryAuthorizer _queryAuthorizer;

    public GetOrderByIdHandler(IQueryAuthorizer queryAuthorizer)
    {
        _queryAuthorizer = queryAuthorizer;
    }

    public Task<IQueryable<Order>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        return Task.FromResult(_queryAuthorizer.GetAuthorizedEntities<Order>().Where(a => a.Id == request.OrderId));
    }
}