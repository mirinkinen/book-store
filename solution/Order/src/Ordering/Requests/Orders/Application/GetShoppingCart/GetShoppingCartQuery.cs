using Common.Application.Authentication;

namespace Ordering.Requests.Orders.Application.GetShoppingCart;

public class GetShoppingCartQuery
{
    public User User { get; }

    public GetShoppingCartQuery(User user)
    {
        User = user;
    }
}