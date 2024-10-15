using InfinityNetServer.BuildingBlocks.Application.Bus;
using MassTransit;
using System.Threading.Tasks;

namespace InfinityNetServer.BuildingBlocks.Infrastructure.Bus;

public class MessageBus : IMessageBus
{

    private readonly IPublishEndpoint _publishEndpoint;

    public MessageBus(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Publish<TEvent>(TEvent @event) where TEvent : IIntegrationEvent
    {
        await _publishEndpoint.Publish(@event);
    }

}
