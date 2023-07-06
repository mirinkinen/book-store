using Common.Application.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Ordering.Application.Requests.GetOrderById;
using Ordering.Application.Requests.GetOrders;
using Ordering.Domain.Orders;
using Wolverine;

namespace Ordering.Api.Orders;

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

    [EnableQuery(PageSize = 20)]
    public Task<IQueryable<Order>> Get()
    {
        var query = new GetOrdersQuery(_userService.GetUser());
        return _bus.InvokeAsync<IQueryable<Order>>(query);
    }

    [EnableQuery]
    public async Task<IActionResult> Get([FromRoute] Guid key)
    {
        var query = new GetOrderByIdQuery(key, _userService.GetUser());
        var OrderQuery = await _bus.InvokeAsync<IQueryable<Order>>(query);

        return Ok(SingleResult.Create(OrderQuery));
    }
}