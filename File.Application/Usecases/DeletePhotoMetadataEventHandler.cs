using InfinityNetServer.BuildingBlocks.Application.Contracts.Events;
using InfinityNetServer.Services.File.Application.IServices;
using InfinityNetServer.Services.File.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.File.Application.Usecases
{
    public class DeletePhotoMetadataEventHandler
        (ILogger<DeletePhotoMetadataEventHandler> logger,
        IMinioClientService minioClientService,
        IPhotoMetadataService photoMetadataService) : IRequestHandler<DomainEvent.DeletePhotoMetadataEvent>
    {

        public async Task Handle(DomainEvent.DeletePhotoMetadataEvent request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling message with file name: {Id}", request.FileMetadataId);
            PhotoMetadata photoMetadata = await photoMetadataService.GetById(request.FileMetadataId.ToString());

            if (photoMetadata != null)
            {
                await photoMetadataService.Delete(photoMetadata.Id.ToString());
                await minioClientService.DeleteObject("infinity-net-bucket", photoMetadata.Name);
            }
        }

    }
}
