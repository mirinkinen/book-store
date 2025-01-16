using Wolverine.Attributes;

namespace Ordering.Requests.Orders.Application.Pong;

using Ping = Common.Application.Messages.Ping;


public class PongHandler
{
    [Transactional] 
    public static Ping Handle(Common.Application.Messages.Pong pong)
    {
        return new Ping(pong.Number + 1);
    }
}