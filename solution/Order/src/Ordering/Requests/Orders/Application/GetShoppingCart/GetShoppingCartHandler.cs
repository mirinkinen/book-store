using Common.Application.Authentication;
using Ordering.Requests.Orders.Application.CreateShoppingCart;
using Ordering.Requests.Orders.Domain.Orders;
using Wolverine;

namespace Ordering.Requests.Orders.Application.GetShoppingCart;

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