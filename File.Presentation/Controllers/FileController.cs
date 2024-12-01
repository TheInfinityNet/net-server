using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Presentation.Controllers;
using InfinityNetServer.Services.File.Application;
using InfinityNetServer.Services.File.Application.Exceptions;
using InfinityNetServer.Services.File.Application.Services;
using InfinityNetServer.Services.File.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static InfinityNetServer.Services.File.Application.Helper.FileHelper;
using static InfinityNetServer.Services.File.Infrastructure.Minio.MinioExtension;

namespace InfinityNetServer.Services.File.Presentation.Controllers
{
    [ApiController]
    public class FileController(
        IAuthenticatedUserService authenticatedUserService,
        ILogger<FileController> logger,
        IStringLocalizer<FileSharedResource> Localizer,
        IMessageBus messageBus,
        IPhotoMetadataService photoMetadataService,
        IVideoMetadataService videoMetadataService,
        IBaseRedisService<string, PhotoMetadata> redisServiceForPhoto,
        IBaseRedisService<string, VideoMetadata> redisServiceForVideo,
        CommonProfileClient profileClient,
        CommonPostClient postClient,
        CommonCommentClient commentClient,
        IMinioClientService minioClientService) : BaseApiController(authenticatedUserService)
    {

        [EndpointDescription("Seed data for post photo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("seed/posts/photos")]
        public async Task<IActionResult> SeedDataForPostPhoto()
        {
            //await minioClientService.DeleteAllObjectsInBucket(MAIN_BUCKET_NAME);
            var fileMetadataIdsWithTypes = await postClient.GetPreviewFileMetadatas(BuildingBlocks.Application.Protos.PostType.Photo);

            foreach (var fileMetadataIdWithType in fileMetadataIdsWithTypes)
            {
                int ramdomIndex = new Random().Next(1, 9);
                string filePath = FAKE_PHOTOS_FOLDER_PATH + $"\\photo{ramdomIndex}.jpg";
                string fileName = GenerateFileName("image", "jpg");
                string contentType = GetContentType(filePath);

                int width;
                int height;
                long size;
                using (FileStream stream = System.IO.File.OpenRead(filePath))
                {
                    (width, height) = await GetImageDimensionsAsync(stream);
                    size = stream.Length;
                    stream.Position = 0;

                    await minioClientService.StoreObject(MAIN_BUCKET_NAME, stream, fileName, contentType);
                }

                await photoMetadataService.Create(new PhotoMetadata
                {
                    Id = Guid.Parse(fileMetadataIdWithType.FileMetadataId),
                    Name = fileName,
                    Width = width,
                    Height = height,
                    Size = size,
                    OwnerType = FileOwnerType.Post,
                    OwnerId = Guid.Parse(fileMetadataIdWithType.Id),
                    CreatedBy = Guid.Parse(fileMetadataIdWithType.OwnerId),
                });
            }

            return Ok(new
            {
                Message = "Data seeded successfully"
            });
        }

        [EndpointDescription("Seed data for post video")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("seed/posts/videos")]
        public async Task<IActionResult> SeedDataForPostVideo()
        {
            //await minioClientService.DeleteAllObjectsInBucket(MAIN_BUCKET_NAME);
            var fileMetadataIdsWithTypes = await postClient.GetPreviewFileMetadatas(BuildingBlocks.Application.Protos.PostType.Video);

            foreach (var fileMetadataIdWithType in fileMetadataIdsWithTypes)
            {
                int ramdomIndex = new Random().Next(1, 9);
                string filePath = FAKE_VIDEOS_FOLDER_PATH + $"\\video{ramdomIndex}.mp4";
                string fileName = GenerateFileName("video", "mp4");
                string contentType = GetContentType(filePath);

                int width;
                int height;
                TimeSpan duration;
                long size;

                (width, height, duration) = await GetVideoInfoAsync(filePath);

                using (FileStream stream = System.IO.File.OpenRead(filePath))
                {
                    (width, height, duration) = await GetVideoInfoAsync(filePath);
                    size = stream.Length;
                    stream.Position = 0;

                    await minioClientService.StoreObject(MAIN_BUCKET_NAME, stream, fileName, contentType);
                }

                var thumbnailPath = await CreateThumbnail(filePath, width, height, TimeSpan.FromSeconds(1));
                var thumbnailName = GenerateFileName("thumbnail", "jpg");
                var thumnailSize = new FileInfo(thumbnailPath).Length;
                using (var thumbnailStream = new FileStream(thumbnailPath, FileMode.Open, FileAccess.Read))
                    await minioClientService.StoreObject(MAIN_BUCKET_NAME, thumbnailStream, thumbnailName, "image/jpg");

                thumbnailPath = Path.ChangeExtension(filePath, ".jpg");
                if (System.IO.File.Exists(thumbnailPath)) System.IO.File.Delete(thumbnailPath);

                await videoMetadataService.Create(new VideoMetadata
                {
                    Id = Guid.Parse(fileMetadataIdWithType.FileMetadataId),
                    Name = fileName,
                    Width = width,
                    Height = height,
                    Size = size,
                    Duration = duration.Seconds,
                    OwnerType = FileOwnerType.Post,
                    OwnerId = Guid.Parse(fileMetadataIdWithType.Id),
                    CreatedBy = Guid.Parse(fileMetadataIdWithType.OwnerId),
                    Thumbnail = new PhotoMetadata
                    {
                        Name = thumbnailName,
                        Width = width,
                        Height = height,
                        Size = thumnailSize,
                        OwnerType = FileOwnerType.Post,
                        OwnerId = Guid.Parse(fileMetadataIdWithType.Id),
                        CreatedBy = Guid.Parse(fileMetadataIdWithType.OwnerId)
                    }
                });
            }

            return Ok(new
            {
                Message = "Data seeded successfully"
            });
        }

        [EndpointDescription("Seed data for comment file")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("seed/comments/photos")]
        public async Task<IActionResult> SeedDataForCommentFile()
        {
            //await minioClientService.DeleteAllObjectsInBucket(MAIN_BUCKET_NAME);
            var fileMetadataIdsWithOwnerIds = await commentClient.GetPreviewFileMetadatas(BuildingBlocks.Application.Protos.PostType.Photo);

            foreach (var fileMetadataIdWithOwnerId in fileMetadataIdsWithOwnerIds)
            {
                int ramdomIndex = new Random().Next(1, 9);
                string filePath = FAKE_PHOTOS_FOLDER_PATH + $"\\photo{ramdomIndex}.jpg";
                string fileName = GenerateFileName("image", "jpg");
                string contentType = GetContentType(filePath);

                int width;
                int height;
                long size;
                using (FileStream stream = System.IO.File.OpenRead(filePath))
                {
                    (width, height) = await GetImageDimensionsAsync(stream);
                    size = stream.Length;
                    stream.Position = 0;

                    await minioClientService.StoreObject(MAIN_BUCKET_NAME, stream, fileName, contentType);
                }

                await photoMetadataService.Create(new PhotoMetadata
                {
                    Id = Guid.Parse(fileMetadataIdWithOwnerId.FileMetadataId),
                    OwnerType = FileOwnerType.Comment,
                    Name = fileName,
                    Width = width,
                    Height = height,
                    Size = size,
                    OwnerId = Guid.Parse(fileMetadataIdWithOwnerId.Id),
                    CreatedBy = Guid.Parse(fileMetadataIdWithOwnerId.OwnerId),
                });
            }

            return Ok(new
            {
                Message = "Data seeded successfully"
            });
        }

        [EndpointDescription("Seed data for comment video")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("seed/comments/videos")]
        public async Task<IActionResult> SeedDataForCommentVideo()
        {
            //await minioClientService.DeleteAllObjectsInBucket(MAIN_BUCKET_NAME);
            var fileMetadataIdsWithTypes = await commentClient.GetPreviewFileMetadatas(BuildingBlocks.Application.Protos.PostType.Video);

            foreach (var fileMetadataIdWithType in fileMetadataIdsWithTypes)
            {
                int ramdomIndex = new Random().Next(1, 9);
                string filePath = FAKE_VIDEOS_FOLDER_PATH + $"\\video{ramdomIndex}.mp4";
                string fileName = GenerateFileName("video", "mp4");
                string contentType = GetContentType(filePath);

                int width;
                int height;
                TimeSpan duration;
                long size;

                (width, height, duration) = await GetVideoInfoAsync(filePath);

                using (FileStream stream = System.IO.File.OpenRead(filePath))
                {
                    (width, height, duration) = await GetVideoInfoAsync(filePath);
                    size = stream.Length;
                    stream.Position = 0;

                    await minioClientService.StoreObject(MAIN_BUCKET_NAME, stream, fileName, contentType);
                }

                var thumbnailPath = await CreateThumbnail(filePath, width, height, TimeSpan.FromSeconds(1));
                var thumbnailName = GenerateFileName("thumbnail", "jpg");
                var thumnailSize = new FileInfo(thumbnailPath).Length;
                using (var thumbnailStream = new FileStream(thumbnailPath, FileMode.Open, FileAccess.Read))
                    await minioClientService.StoreObject(MAIN_BUCKET_NAME, thumbnailStream, thumbnailName, "image/jpg");

                thumbnailPath = Path.ChangeExtension(filePath, ".jpg");
                if (System.IO.File.Exists(thumbnailPath)) System.IO.File.Delete(thumbnailPath);

                await videoMetadataService.Create(new VideoMetadata
                {
                    Id = Guid.Parse(fileMetadataIdWithType.FileMetadataId),
                    Name = fileName,
                    Width = width,
                    Height = height,
                    Size = size,
                    Duration = duration.Seconds,
                    OwnerType = FileOwnerType.Comment,
                    OwnerId = Guid.Parse(fileMetadataIdWithType.Id),
                    CreatedBy = Guid.Parse(fileMetadataIdWithType.OwnerId),
                    Thumbnail = new PhotoMetadata
                    {
                        Name = thumbnailName,
                        Width = width,
                        Height = height,
                        Size = thumnailSize,
                        OwnerType = FileOwnerType.Comment,
                        OwnerId = Guid.Parse(fileMetadataIdWithType.Id),
                        CreatedBy = Guid.Parse(fileMetadataIdWithType.OwnerId)
                    }
                });
            }

            return Ok(new
            {
                Message = "Data seeded successfully"
            });
        }

        [EndpointDescription("Seed data for profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("seed/profiles")]
        public async Task<IActionResult> SeedDataForProfiles()
        {
            //await minioClientService.DeleteAllObjectsInBucket(MAIN_BUCKET_NAME);
            var fileMetadataIdsWithOwnerIds = await profileClient.GetPreviewFileMetadatas();

            foreach (var fileMetadataIdWithOwnerId in fileMetadataIdsWithOwnerIds)
            {
                int ramdomIndex = new Random().Next(1, 9);
                string filePath = FAKE_PHOTOS_FOLDER_PATH + $"\\photo{ramdomIndex}.jpg";
                string fileName = GenerateFileName("image", "jpg");
                string contentType = GetContentType(filePath);

                int width;
                int height;
                long size;
                using (FileStream stream = System.IO.File.OpenRead(filePath))
                {
                    (width, height) = await GetImageDimensionsAsync(stream);
                    size = stream.Length;
                    stream.Position = 0;

                    await minioClientService.StoreObject(MAIN_BUCKET_NAME, stream, fileName, contentType);
                }

                await photoMetadataService.Create(new PhotoMetadata
                {
                    Id = Guid.Parse(fileMetadataIdWithOwnerId.FileMetadataId),
                    OwnerType = FileOwnerType.Profile,
                    Name = fileName,
                    Width = width,
                    Height = height,
                    Size = size,
                    OwnerId = Guid.Parse(fileMetadataIdWithOwnerId.Id),
                    CreatedBy = Guid.Parse(fileMetadataIdWithOwnerId.OwnerId),
                });
            }

            return Ok(new
            {
                Message = "Data seeded successfully"
            });
        }

        [EndpointDescription("Upload raw file")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadRawFile(IFormFile file)
        {
            ValidateFile(file);

            await using var stream = file.OpenReadStream();
            var filetype = file.ContentType.Split('/').First();
            var extension = file.FileName.Split('.').Last();
            await minioClientService.StoreObject(
                MAIN_BUCKET_NAME, stream, GenerateFileName(filetype, extension), file.ContentType);

            return Ok(new
            {
                fileName = file.FileName,
                size = file.Length,
                mimeType = file.ContentType
            });
        }

        [EndpointDescription("Upload raw image")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        [HttpPost("upload/photo")]
        public async Task<IActionResult> UploadRawImage(IFormFile photo, [FromForm] bool isTemporarily = false)
        {
            ValidateImage(photo);

            int width;
            int height;
            long size = photo.Length;

            var filetype = photo.ContentType.Split('/').First();
            var extension = photo.ContentType.Split('/').Last();
            var fileName = GenerateFileName(filetype, extension);
            var id = Guid.NewGuid();

            await using (var stream = photo.OpenReadStream())
            {
                (width, height) = await GetImageDimensionsAsync(stream);

                // Reset stream position to the beginning
                stream.Position = 0;

                await minioClientService.StoreObject(isTemporarily 
                    ? TEMP_BUCKET_NAME : MAIN_BUCKET_NAME, stream, fileName, photo.ContentType);

                if (isTemporarily)
                    await redisServiceForPhoto.SetWithExpirationAsync(id.ToString(), new PhotoMetadata
                    {
                        Id = id,
                        Name = fileName,
                        Width = width,
                        Height = height,
                        Size = size,
                        CreatedBy = GetCurrentProfileId(),
                    }, TimeSpan.FromMinutes(30));
            }

            return Ok(new
            {
                id,
                fileName,
                size,
                width,
                height

            });
        }

        [EndpointDescription("Upload raw video")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        [HttpPost("upload/video")]
        public async Task<IActionResult> UploadRawVideo(
            IFormFile video,
            [FromForm]
            [Required(ErrorMessage = "Required.ThumbnailWidth")]
            [Range(1, 1920, ErrorMessage = "Range.ThumbnailWidth")]
            int thumbnailWidth,

            [FromForm]
            [Required(ErrorMessage = "Required.ThumbnailHeight")]
            [Range(1, 1080, ErrorMessage = "Range.ThumbnailHeight")]
            int thumbnailHeight,

            [FromForm]
            bool isTemporarily = false)
        {
            if (video == null || video.Length == 0)
                throw new FileException(FileError.FILE_EMPTY, StatusCodes.Status400BadRequest);

            var filetype = video.ContentType.Split('/').First();
            var extension = video.ContentType.Split('/').Last();
            var id = Guid.NewGuid();
            long size = video.Length;

            var tempFilePath = Path.GetTempFileName();
            await using (var stream = new FileStream(tempFilePath, FileMode.Create))
                await video.CopyToAsync(stream);

            try
            {
                // Lấy thông tin chiều rộng, chiều cao của video
                var (width, height, duration) = await GetVideoInfoAsync(tempFilePath);

                // Tạo thumbnail từ video với kích thước tùy chỉnh
                var thumbnailTime = TimeSpan.FromSeconds(1);
                var thumbnailPath = await CreateThumbnail(tempFilePath, thumbnailWidth, thumbnailHeight, thumbnailTime);

                // Lưu video lên MinIO
                var videoFileName = GenerateFileName(filetype, extension);
                await using (var videoStream = video.OpenReadStream())
                    await minioClientService.StoreObject(isTemporarily 
                        ? TEMP_BUCKET_NAME : MAIN_BUCKET_NAME, videoStream, videoFileName, video.ContentType);

                // Lưu thumbnail vào MinIO
                var thumbnailFileName = GenerateFileName("thumbnail", "jpg");
                long thumbnailSize = new FileInfo(thumbnailPath).Length;
                await using (var thumbnailStream = new FileStream(thumbnailPath, FileMode.Open, FileAccess.Read))
                    await minioClientService.StoreObject(isTemporarily 
                        ? TEMP_BUCKET_NAME : MAIN_BUCKET_NAME, thumbnailStream, thumbnailFileName, "image/jpg");

                if (isTemporarily)
                    await redisServiceForVideo.SetWithExpirationAsync(id.ToString(), new VideoMetadata
                    {
                        Id = id,
                        Name = videoFileName,
                        Width = width,
                        Height = height,
                        Size = size,
                        Duration = duration.Seconds,
                        CreatedBy = GetCurrentProfileId(),
                        Thumbnail = new PhotoMetadata
                        {
                            Name = thumbnailFileName,
                            Width = width,
                            Height = height,
                            Size = thumbnailSize,
                            CreatedBy = GetCurrentProfileId()
                        }
                    }, TimeSpan.FromMinutes(30));

                return Ok(new
                {
                    id,
                    fileName = videoFileName,
                    size,
                    width,
                    height,
                    duration,
                    thumbnail = new
                    {
                        name = thumbnailFileName,
                        width = thumbnailWidth,
                        height = thumbnailHeight,
                        size = thumbnailSize
                    }
                });
            }
            catch (Exception ex)
            {
                logger.LogError($"Can not save file caused by: {ex.Message}");
                throw new FileException(FileError.CAN_NOT_STORE_FILE, StatusCodes.Status422UnprocessableEntity);
            }
            finally
            {
                if (System.IO.File.Exists(tempFilePath)) System.IO.File.Delete(tempFilePath);

                var thumbnailPath = Path.ChangeExtension(tempFilePath, ".jpg");
                if (System.IO.File.Exists(thumbnailPath)) System.IO.File.Delete(thumbnailPath);
            }
        }

    }
}
