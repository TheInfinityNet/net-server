using InfinityNetServer.BuildingBlocks.Application.Consumers;
using InfinityNetServer.BuildingBlocks.Application.Contracts.Events;
using MediatR;

namespace InfinityNetServer.Services.File.Application.Consumers
{
    public class DeletePhotoMetadataEventConsumer
        (ISender sender) : BaseConsumer<DomainEvent.DeletePhotoMetadataEvent>(sender)
    {

    }
}
