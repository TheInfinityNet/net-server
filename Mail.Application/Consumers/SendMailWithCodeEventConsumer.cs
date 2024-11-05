using InfinityNetServer.BuildingBlocks.Application.Consumers;
using InfinityNetServer.BuildingBlocks.Application.Contracts.Events;
using MediatR;

namespace InfinityNetServer.Services.Mail.Application.Consumers
{
    public class SendMailWithCodeEventConsumer(ISender sender) : BaseConsumer<DomainEvent.SendMailWithCodeEvent>(sender)
    {

    }
}
