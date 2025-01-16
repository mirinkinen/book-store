using Common.Api.Application.Authentication;

namespace Ordering.Application.Requests.CreateShoppingCart;

public class CreateShoppingCartCommand
{
    public User User { get; }

    public CreateShoppingCartCommand(User user)
    {
        User = user;
        throw new NotImplementedException();
    }
}