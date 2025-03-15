using Common.Application.Authentication;
using Ordering.Domain;

namespace Ordering.Application.GetOrderById;

public record GetOrderByIdQuery(Guid OrderId);

public class GetOrderByIdHandler
{
    private readonly IReadOnlyDbContext _readOnlyDbContext;
    private readonly IUserAccessor _userAccessor;

    public GetOrderByIdHandler(IReadOnlyDbContext readOnlyDbContext, IUserAccessor userAccessor)
    {
        _readOnlyDbContext = readOnlyDbContext;
        _userAccessor = userAccessor;
    }

    public async Task<IQueryable<Order>> Handle(GetOrderByIdQuery request)
    {
        var user = await _userAccessor.GetUser();

        return _readOnlyDbContext.GetOrders(user).Where(a => a.Id == request.OrderId);
    }
}