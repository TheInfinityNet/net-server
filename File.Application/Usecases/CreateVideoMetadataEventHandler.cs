
using InfinityNetServer.BuildingBlocks.Application.Contracts.Events;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.Services.File.Application.Services;
using InfinityNetServer.Services.File.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.File.Application.Usecases
{
    public class CreateVideoMetadataEventHandler
        (ILogger<CreateVideoMetadataEventHandler> logger,
        IBaseRedisService<string, VideoMetadata> baseRedisService,
        IMinioClientService minioClientService,
        CommonPostClient postClient,
        IVideoMetadataService videoMetadataService) : IRequestHandler<DomainEvent.VideoMetadataEvent>
    {

        public async Task Handle(DomainEvent.VideoMetadataEvent request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling message with file name: {Id}", request.Id);

            string tempId = request.Id.Equals(request.TempId) ? request.Id.ToString() : request.TempId.ToString();

            VideoMetadata videoMetadata = await baseRedisService.GetAsync(tempId);

            var previewPostResponse = await postClient.GetPreviewPost(request.OwnerId.ToString());
            logger.LogInformation("File metadata id: {fileMetadataId}", previewPostResponse.FileMetadataId.Value);

            if (previewPostResponse.FileMetadataId == null)
            {
                await videoMetadataService.Create(new VideoMetadata
                {
                    Id = request.Id,
                    Type = FileMetadataType.Photo,
                    Name = videoMetadata.Name,
                    Width = videoMetadata.Width,
                    Height = videoMetadata.Height,
                    Size = videoMetadata.Size,
                    Duration = videoMetadata.Duration,
                    OwnerType = request.OwnerType,
                    OwnerId = request.OwnerId,
                    CreatedBy = videoMetadata.CreatedBy,
                    CreatedAt = videoMetadata.CreatedAt,
                    UpdatedBy = request.UpdatedBy,
                    UpdatedAt = request.UpdatedAt,
                    Thumbnail = videoMetadata.Thumbnail
                });
                
            }
            else
            {
                VideoMetadata existedVideoMetadata = await videoMetadataService.GetById(previewPostResponse.FileMetadataId.Value.ToString());
                await minioClientService.DeleteObject("infinity-net-bucket", existedVideoMetadata.Name);
                await minioClientService.DeleteObject("infinity-net-bucket", existedVideoMetadata.Thumbnail.Name);

                existedVideoMetadata.Name = videoMetadata.Name;
                existedVideoMetadata.Width = videoMetadata.Width;
                existedVideoMetadata.Height = videoMetadata.Height;
                existedVideoMetadata.Size = videoMetadata.Size;
                existedVideoMetadata.Duration = videoMetadata.Duration;
                existedVideoMetadata.OwnerType = request.OwnerType;
                existedVideoMetadata.UpdatedAt = request.UpdatedAt;
                existedVideoMetadata.UpdatedBy = request.UpdatedBy;
                existedVideoMetadata.Thumbnail = videoMetadata.Thumbnail;

                await videoMetadataService.Update(existedVideoMetadata);
            }

            await baseRedisService.DeleteAsync(tempId);
            await minioClientService.CopyObject("infinity-net-temp-bucket", videoMetadata.Name, "infinity-net-bucket", videoMetadata.Name);
            await minioClientService.CopyObject("infinity-net-temp-bucket", videoMetadata.Thumbnail.Name, "infinity-net-bucket", videoMetadata.Thumbnail.Name);
            await minioClientService.DeleteObject("infinity-net-temp-bucket", videoMetadata.Name);
            await minioClientService.DeleteObject("infinity-net-temp-bucket", videoMetadata.Thumbnail.Name);
        }

    }
}
