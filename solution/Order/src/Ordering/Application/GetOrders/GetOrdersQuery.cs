using Common.Application.Authentication;
using Ordering.Domain;

namespace Ordering.Application.GetOrders;

public record GetOrdersQuery();

public class GetOrdersHandler
{
    private readonly IReadOnlyDbContext _readOnlyDbContext;
    private readonly IUserAccessor _userAccessor;

    public GetOrdersHandler(IReadOnlyDbContext readOnlyDbContext, IUserAccessor userAccessor)
    {
        _readOnlyDbContext = readOnlyDbContext;
        _userAccessor = userAccessor;
    }

    public async Task<IQueryable<Order>> Handle(GetOrdersQuery request)
    {
        var user = await _userAccessor.GetUser();
        return _readOnlyDbContext.GetOrders(user);
    }
}