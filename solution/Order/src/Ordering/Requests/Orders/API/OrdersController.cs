using Common.Application.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Ordering.Requests.Orders.Application.GetOrderById;
using Ordering.Requests.Orders.Application.GetOrders;
using Ordering.Requests.Orders.Domain.Orders;
using Wolverine;

namespace Ordering.Requests.Orders.API;

[ODataRouteComponent("v1")]
public partial class OrdersController : ODataController
{
    private readonly IMessageBus _bus;
    private readonly IUserService _userService;

    public OrdersController(IMessageBus bus, IUserService userService)
    {
        _bus = bus;
        _userService = userService;
    }

    // [Route("shoppingcart")]
    // public Task<Order> GetShoppingCart()
    // {
    //     var query = new GetShoppingCartQuery(_userService.GetUser());
    //     return _bus.InvokeAsync<Order>(query);
    // }
    //
    
    [EnableQuery(PageSize = 20)]
    public async Task<IQueryable<Order>> Get()
    {
        var query = new GetOrdersQuery();
        return await _bus.InvokeAsync<IQueryable<Order>>(query);
    }

    [EnableQuery]
    public async Task<IActionResult> Get([FromRoute] Guid key)
    {
        var query = new GetOrderByIdQuery(key);
        var orderQuery = await _bus.InvokeAsync<IQueryable<Order>>(query);

        return Ok(SingleResult.Create(orderQuery));
    }
}