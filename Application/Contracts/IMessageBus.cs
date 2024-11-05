using InfinityNetServer.BuildingBlocks.Application.Contracts.Messages;
using System.Threading.Tasks;

namespace InfinityNetServer.BuildingBlocks.Application.Contracts;

public interface IMessageBus
{

    Task Publish<TEvent>(TEvent @event) where TEvent : IMessage;

}
