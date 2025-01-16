using Common.Api.Application.Authentication;
using Ordering.Application.Requests.CreateShoppingCart;
using Ordering.Domain.Orders;
using Wolverine;

namespace Ordering.Application.Requests.GetShoppingCart;

public static class GetShoppingCartHandler
{
    public static async Task<Order> Handle(User user, IOrderRepository orderRepository, IMessageContext messageContext)
    {
        var shoppingCart = await orderRepository.GetShoppingCart();

        if (shoppingCart is not null)
        {
            return shoppingCart;
        }

        var createShoppingCartCommand = new CreateShoppingCartCommand(user);
        shoppingCart = await messageContext.InvokeAsync<Order>(createShoppingCartCommand);

        return shoppingCart;
    }
}