using System.Threading.Tasks;
using InfinityNetServer.BuildingBlocks.Application.Contracts.Messages;
using MassTransit;
using MediatR;

namespace InfinityNetServer.BuildingBlocks.Application.Consumers;

public abstract class BaseConsumer<TEvent>(ISender sender) : IConsumer<TEvent> where TEvent : class, IMessage
{

    public async Task Consume(ConsumeContext<TEvent> context)
        => await sender.Send(context.Message);

}
