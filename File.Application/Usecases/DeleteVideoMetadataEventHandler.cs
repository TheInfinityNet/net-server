using InfinityNetServer.BuildingBlocks.Application.Contracts.Events;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.Services.File.Application.IServices;
using InfinityNetServer.Services.File.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.File.Application.Usecases
{
    public class DeleteVideoMetadataEventHandler
        (ILogger<DeleteVideoMetadataEventHandler> logger,
        IMinioClientService minioClientService,
        IVideoMetadataService videoMetadataService) : IRequestHandler<DomainEvent.DeleteVideoMetadataEvent>
    {

        public async Task Handle(DomainEvent.DeleteVideoMetadataEvent request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling message with file name: {Id}", request.FileMetadataId);
            VideoMetadata videoMetadata = await videoMetadataService.GetById(request.FileMetadataId.ToString()) 
                ?? throw new BaseException(BaseError.FILE_NOT_FOUND, StatusCodes.Status404NotFound);

            await videoMetadataService.Delete(videoMetadata.Id.ToString());
            await minioClientService.DeleteObject("infinity-net-bucket", videoMetadata.Name);
        }

    }
}
