using Common.Application.Authentication;

namespace Ordering.Application.CreateShoppingCart;

public class CreateShoppingCartCommand
{
    public User User { get; }

    public CreateShoppingCartCommand(User user)
    {
        User = user;
        throw new NotImplementedException();
    }
}