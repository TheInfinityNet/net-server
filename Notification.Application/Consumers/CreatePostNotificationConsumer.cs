using InfinityNetServer.BuildingBlocks.Application.Consumers;
using InfinityNetServer.BuildingBlocks.Application.Contracts.Commands;
using MediatR;

namespace InfinityNetServer.Services.Notification.Application.Consumers
{
    public class CreatePostNotificationConsumer
        (ISender sender) : BaseConsumer<DomainCommand.CreatePostNotificationCommand>(sender)
    {

    }
}
