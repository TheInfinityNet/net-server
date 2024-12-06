using InfinityNetServer.BuildingBlocks.Application.Consumers;
using InfinityNetServer.BuildingBlocks.Application.Contracts.Commands;
using MediatR;

namespace InfinityNetServer.Services.Mail.Application.Consumers
{
    public class SendMailWithCodeConsumer
        (ISender sender) : BaseConsumer<DomainCommand.SendMailWithCodeCommand>(sender)
    {

    }
}
