using Microsoft.AspNetCore.Mvc;
using RabbitMQ.MassTransit.Producer.Events.Senders;

namespace RabbitMQ.MassTransit.Producer.Controllers;
[Route("api/ping")]
[ApiController]
public class PingController : ControllerBase
{

    private readonly IPingEventSender _sender;
    public PingController(IPingEventSender sender)
    {
        _sender = sender;
    }
    [HttpGet()]
    public async Task Ping(string message)
    {
        await _sender.SendPingEvent(new RabbitMq.Contracts.PingEvent()
        {
            Message = message
        });
    }
}
