
using InfinityNetServer.BuildingBlocks.Application.Contracts.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.File.Application.Usecases
{
    public class CreatePhotoMetadataEventHandler
        (ILogger<CreatePhotoMetadataEventHandler> logger) : IRequestHandler<DomainEvent.PhotoMetadataEvent>
    {

        public Task Handle(DomainEvent.PhotoMetadataEvent request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling message with file name: {Name}", request.Name);
            return Task.CompletedTask;
        }

    }
}
