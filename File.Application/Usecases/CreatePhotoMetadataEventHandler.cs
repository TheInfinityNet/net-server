
using InfinityNetServer.BuildingBlocks.Application.Contracts.Events;
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
    public class CreatePhotoMetadataEventHandler
        (ILogger<CreatePhotoMetadataEventHandler> logger,
        IBaseRedisService<string, PhotoMetadata> baseRedisService,
        IMinioClientService minioClientService,
        IPhotoMetadataService photoMetadataService) : IRequestHandler<DomainEvent.PhotoMetadataEvent>
    {

        public async Task Handle(DomainEvent.PhotoMetadataEvent request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling message with file name: {Id}", request.FileMetadataId);

            PhotoMetadata photoMetadata = await baseRedisService.GetAsync(request.TempId.ToString());

            PhotoMetadata existedPhotoMetadata = await photoMetadataService.GetById(request.FileMetadataId.ToString());
            if (existedPhotoMetadata == null)
            {
                await photoMetadataService.Create(new PhotoMetadata
                {
                    Id = request.FileMetadataId,
                    Type = FileMetadataType.Photo,
                    Name = photoMetadata.Name,
                    Width = photoMetadata.Width,
                    Height = photoMetadata.Height,
                    Size = photoMetadata.Size,
                    OwnerType = request.OwnerType,
                    OwnerId = request.OwnerId,
                    CreatedBy = photoMetadata.CreatedBy,
                    CreatedAt = photoMetadata.CreatedAt,
                    UpdatedBy = request.UpdatedBy,
                    UpdatedAt = request.UpdatedAt
                });

            }
            else
            {
                await minioClientService.DeleteObject("infinity-net-bucket", existedPhotoMetadata.Name);
                existedPhotoMetadata.Name = photoMetadata.Name;
                existedPhotoMetadata.Width = photoMetadata.Width;
                existedPhotoMetadata.Height = photoMetadata.Height;
                existedPhotoMetadata.Size = photoMetadata.Size;
                existedPhotoMetadata.OwnerType = request.OwnerType;
                existedPhotoMetadata.UpdatedAt = request.UpdatedAt;
                existedPhotoMetadata.UpdatedBy = request.UpdatedBy;
                await photoMetadataService.Update(existedPhotoMetadata);
            }

            await baseRedisService.DeleteAsync(request.TempId.ToString());
            await minioClientService.CopyObject("infinity-net-temp-bucket", photoMetadata.Name, "infinity-net-bucket", photoMetadata.Name);
            await minioClientService.DeleteObject("infinity-net-temp-bucket", photoMetadata.Name);
        }

    }
}
