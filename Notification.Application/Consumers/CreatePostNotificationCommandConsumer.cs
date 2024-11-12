using InfinityNetServer.BuildingBlocks.Application.Consumers;
using InfinityNetServer.BuildingBlocks.Application.Contracts.Commands;
using InfinityNetServer.BuildingBlocks.Application.Contracts.Events;
using MediatR;

namespace InfinityNetServer.Services.Notification.Application.Consumers
{
    public class CreatePostNotificationCommandConsumer
        (ISender sender) : BaseConsumer<DomainCommand.PostNotificationCommand>(sender)
    {

    }
}
