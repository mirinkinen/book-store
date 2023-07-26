using Common.Application.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Wolverine;

namespace Cataloging.Api.Pings;

[ODataRouteComponent("v1")]
public class PingsController : ODataController
{
    [HttpPost]
    [Route("ping")]
    public async Task<IActionResult> Ping([FromServices] IMessageBus messageBus)
    {
        await messageBus.SendAsync(new Ping(0));

        return Ok();
    }
}