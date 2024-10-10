using MassTransit;
using System.Threading.Tasks;
using InfinityNetServer.BuildingBlocks.Application.DTOs;

namespace InfinityNetServer.BuildingBlocks.Application.Bus;

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
