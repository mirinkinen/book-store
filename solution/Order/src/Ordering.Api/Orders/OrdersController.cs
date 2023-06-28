using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Ordering.Application.Requests.GetOrderById;
using Ordering.Application.Requests.GetOrders;
using Ordering.Domain.Orders;
using Shared.Application.Authentication;

namespace Ordering.Api.Orders;

[ODataRouteComponent("v1")]
public partial class OrdersController : ODataController
{
    private readonly IMediator _mediatr;
    private readonly IUserService _userService;

    public OrdersController(IMediator mediatr, IUserService userService)
    {
        _mediatr = mediatr;
        _userService = userService;
    }

    [EnableQuery(PageSize = 20)]
    public Task<IQueryable<Order>> Get()
    {
        var query = new GetOrdersQuery(_userService.GetUser());
        return _mediatr.Send(query);
    }

    [EnableQuery]
    public async Task<IActionResult> Get([FromRoute] Guid key)
    {
        var query = new GetOrderByIdQuery(key, _userService.GetUser());
        var OrderQuery = await _mediatr.Send(query);

        return Ok(SingleResult.Create(OrderQuery));
    }
}