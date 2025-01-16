using Common.Api.Application.Authentication;

namespace Ordering.Application.Requests.GetShoppingCart;

public class GetShoppingCartQuery
{
    public User User { get; }

    public GetShoppingCartQuery(User user)
    {
        User = user;
    }
}