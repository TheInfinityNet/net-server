using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.Contracts.Messages;
using MassTransit;
using System.Threading.Tasks;

namespace InfinityNetServer.BuildingBlocks.Infrastructure.Bus;

public class MessageBus(IPublishEndpoint publishEndpoint) : IMessageBus
{

    public async Task Publish<TEvent>(TEvent @event) where TEvent : IMessage
        => await publishEndpoint.Publish(@event);

}
