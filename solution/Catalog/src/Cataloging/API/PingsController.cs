using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Cataloging.API;

[ApiController]
public class PingsController : ControllerBase
{
    [HttpPost]
    [Route("ping")]
    public async Task<IActionResult> Ping([FromServices] IMessageBus messageBus)
    {
        await messageBus.SendAsync(new Common.Application.Messages.Ping(0));

        return Ok();
    }
}