using InfinityNetServer.BuildingBlocks.Application.Consumers;
using InfinityNetServer.BuildingBlocks.Application.Contracts.Commands;
using MediatR;

namespace InfinityNetServer.Services.Notification.Application.Consumers
{
    public class CreateProfileFollowNotificationCommandConsumer
        (ISender sender) : BaseConsumer<DomainCommand.ProfileFollowNotificationCommand>(sender)
    {

    }
}
