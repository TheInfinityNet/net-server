using System.Threading.Tasks;
using InfinityNetServer.BuildingBlocks.Application.DTOs;

namespace InfinityNetServer.BuildingBlocks.Application.Bus;

public interface IMessageBus
{

    Task Publish<TEvent>(TEvent @event) where TEvent : IIntegrationEvent;

}
