using MassTransit;
using RabbitMq.Contracts;

namespace RabbitMQ.MassTransit.Producer.Events.Senders;

public class PingEventSender(ISendEndpointProvider sender) : IPingEventSender
{
    private readonly ISendEndpointProvider _sender = sender;

    public async Task SendPingEvent(PingEvent @event)
    {
        var endpoint = await _sender.GetSendEndpoint(new Uri("queue:mstransit-queue"));

        await endpoint.Send(@event);
    }
}

public interface IPingEventSender
{
    Task SendPingEvent(PingEvent @event);
}
