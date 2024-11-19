using InfinityNetServer.BuildingBlocks.Application.Consumers;
using InfinityNetServer.BuildingBlocks.Application.Contracts.Commands;
using MediatR;

namespace InfinityNetServer.Services.Post.Application.Consumers
{
    public class UpdateUserTimelineConsumer
        (ISender sender) : BaseConsumer<DomainCommand.PushPostToTimelineCommand>(sender)
    {

    }
}
