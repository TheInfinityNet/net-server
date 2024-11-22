using InfinityNetServer.BuildingBlocks.Application.Consumers;
using InfinityNetServer.BuildingBlocks.Application.Contracts.Commands;
using MediatR;

namespace InfinityNetServer.Services.Notification.Application.Consumers
{
    public class CreateCommentNotificationConsumer
        (ISender sender) : BaseConsumer<DomainCommand.CreateCommentNotificationCommand>(sender)
    {

    }
}
