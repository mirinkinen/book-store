using Wolverine.Attributes;

namespace Ordering.Application.Requests.Pong;

using Ping = Common.Api.Application.Messages.Ping;


public class PongHandler
{
    [Transactional] 
    public static Ping Handle(Common.Api.Application.Messages.Pong pong)
    {
        return new Ping(pong.Number + 1);
    }
}