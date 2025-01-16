using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Wolverine;

namespace Cataloging.Requests.Ping.API;

[ODataRouteComponent("v1")]
public class PingsController : ODataController
{
    [HttpPost]
    [Route("ping")]
    public async Task<IActionResult> Ping([FromServices] IMessageBus messageBus)
    {
        await messageBus.SendAsync(new Common.Application.Messages.Ping(0));

        return Ok();
    }
}