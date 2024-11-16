using InfinityNetServer.BuildingBlocks.Application.Consumers;
using InfinityNetServer.BuildingBlocks.Application.Contracts.Commands;
using MediatR;

namespace InfinityNetServer.Services.Notification.Application.Consumers
{
    public class CreateProfileFollowNotificationConsumer
        (ISender sender) : BaseConsumer<DomainCommand.CreateProfileFollowNotificationCommand>(sender)
    {

    }
}
