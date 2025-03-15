using Common.Application.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Ordering.Application.GetOrderById;
using Ordering.Application.GetOrders;
using Ordering.Domain;
using Wolverine;

namespace Ordering.API;

[ODataRouteComponent("v1")]
public partial class OrdersController : ODataController
{
    private readonly IMessageBus _bus;
    private readonly IUserAccessor _userAccessor;

    public OrdersController(IMessageBus bus, IUserAccessor userAccessor)
    {
        _bus = bus;
        _userAccessor = userAccessor;
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