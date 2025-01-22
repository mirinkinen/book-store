using Common.Application.Authentication;

namespace Ordering.Application.GetShoppingCart;

public class GetShoppingCartQuery
{
    public User User { get; }

    public GetShoppingCartQuery(User user)
    {
        User = user;
    }
}