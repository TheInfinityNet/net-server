using System.Threading.Tasks;

namespace InfinityNetServer.BuildingBlocks.Application.Bus;

public interface IMessageBus
{

    Task Publish<TEvent>(TEvent @event) where TEvent : IIntegrationEvent;

}
