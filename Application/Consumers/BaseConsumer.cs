using System.Threading.Tasks;
using MassTransit;

namespace InfinityNetServer.BuildingBlocks.Application.Consumers;

public abstract class BaseConsumer<TEvent> : IConsumer<TEvent> where TEvent : class
{

    public abstract Task ConsumeMessage(ConsumeContext<TEvent> context);

    public async Task Consume(ConsumeContext<TEvent> context)
    {
        await ConsumeMessage(context);
    }

}
