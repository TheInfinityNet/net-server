using FFMpegCore;
using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Presentation.Controllers;
using InfinityNetServer.Services.File.Application;
using InfinityNetServer.Services.File.Application.Services;
using InfinityNetServer.Services.File.Domain.Entities;
using InfinityNetServer.Services.File.Domain.Repositories;
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
        IPhotoMetadataRepository photoMetadataRepository,
        CommonPostClient postClient,
        CommonCommentClient commentClient,
        IMinioClientService minioClientService,
        IBaseRedisService<string, PhotoMetadata> baseRedisService) : BaseApiController(authenticatedUserService) 
    {

        [HttpGet("seed/posts/{type}")]
        public async Task<IActionResult> SeedDataForPostFile(string type)
        {
            await minioClientService.DeleteAllObjectsInBucket(MAIN_BUCKET_NAME);
            var fileMetadataIdsWithTypes = await postClient.GetFileMetadataIdsWithTypes(type);

            foreach (var fileMetadataIdWithType in fileMetadataIdsWithTypes)
            {
                int ramdomIndex = new Random().Next(1, 9);
                string filePath = FAKE_PHOTOS_FOLDER_PATH + $"\\photo{ramdomIndex}.jpg";
                string fileName = GenerateFileName("image", "jpg");
                string contentType = GetContentType(filePath);

                int width;
                int height;
                using (FileStream stream = System.IO.File.OpenRead(filePath))
                {
                    (width, height) = await GetImageDimensionsAsync(stream);
                    stream.Position = 0;

                    await minioClientService.StoreObject(MAIN_BUCKET_NAME, stream, fileName, contentType);
                }

                await photoMetadataRepository.CreateAsync(new PhotoMetadata
                {
                    Id = Guid.Parse(fileMetadataIdWithType.FileMetadataId),
                    Type = Enum.Parse<FileMetadataType>(fileMetadataIdWithType.Type),
                    Name = fileName,
                    Width = width,
                    Height = height,
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

        [HttpGet("seed/comments")]
        public async Task<IActionResult> SeedDataForCommentFile()
        {
            //await minioClientService.DeleteAllObjectsInBucket(MAIN_BUCKET_NAME);
            var fileMetadataIdsWithOwnerIds = await commentClient.GetFileMetadataIdsWithOwnerIds();

            foreach (var fileMetadataIdWithOwnerId in fileMetadataIdsWithOwnerIds)
            {
                int ramdomIndex = new Random().Next(1, 9);
                string filePath = FAKE_PHOTOS_FOLDER_PATH + $"\\photo{ramdomIndex}.jpg";
                string fileName = GenerateFileName("image", "jpg");
                string contentType = GetContentType(filePath);

                int width;
                int height;
                using (FileStream stream = System.IO.File.OpenRead(filePath))
                {
                    (width, height) = await GetImageDimensionsAsync(stream);
                    stream.Position = 0;

                    await minioClientService.StoreObject(MAIN_BUCKET_NAME, stream, fileName, contentType);
                }

                await photoMetadataRepository.CreateAsync(new PhotoMetadata
                {
                    Id = Guid.Parse(fileMetadataIdWithOwnerId.FileMetadataId),
                    Type = FileMetadataType.Photo,
                    Name = fileName,
                    Width = width,
                    Height = height,
                    OwnerId = Guid.Parse(fileMetadataIdWithOwnerId.Id),
                    CreatedBy = Guid.Parse(fileMetadataIdWithOwnerId.OwnerId),
                });
            }

            return Ok(new
            {
                Message = "Data seeded successfully"
            });
        }

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

        [Authorize]
        [HttpPost("upload/photo")]
        public async Task<IActionResult> UploadRawImage(IFormFile photo, [FromForm] bool isTemporarily = false)
        {
            ValidateImage(photo);

            int width;
            int height;
            var filetype = photo.ContentType.Split('/').First();
            var extension = photo.ContentType.Split('/').Last();
            var fileName = GenerateFileName(filetype, extension);
            var id = Guid.NewGuid();

            await using (var stream = photo.OpenReadStream())
            {
                (width, height) = await GetImageDimensionsAsync(stream);

                // Reset stream position to the beginning
                stream.Position = 0;

                await minioClientService.StoreObject(isTemporarily ? TEMP_BUCKET_NAME : MAIN_BUCKET_NAME, 
                    stream, fileName, photo.ContentType);

                if (isTemporarily) 
                    await baseRedisService.SetWithExpirationAsync(id.ToString(), new PhotoMetadata
                        {
                            Id = id,
                            Type = FileMetadataType.Photo,
                            Name = fileName,
                            Width = width,
                            Height = height,
                            CreatedBy = GetCurrentUserId(),
                        }, TimeSpan.FromMinutes(30));
            }

            return Ok(new
            {
                id,
                fileName,
                size = photo.Length,
                width,
                height

            });
        }

        [HttpPost("upload/video")]
        public async Task<IActionResult> UploadVideo(
            IFormFile video, 

            [FromForm]
            [Required(ErrorMessage = "null_thumbnail_width")]
            [Range(1, 1920, ErrorMessage = "invalid_thumbnail_width")]
            int thumbnailWidth, 

            [FromForm] 
            [Required(ErrorMessage = "null_thumbnail_height")]
            [Range(1, 1080, ErrorMessage = "invalid_thumbnail_height")]
            int thumbnailHeight)
        {
            // Validate video
            ValidateVideo(video);

            int width = 0;
            int height = 0;

            // Tạo file tạm cho video
            var tempFilePath = Path.GetTempFileName();
            await using (var stream = new FileStream(tempFilePath, FileMode.Create))
            await video.CopyToAsync(stream);

            // Phân tích video và lấy thông tin chiều rộng và chiều cao
            var videoInfo = await FFProbe.AnalyseAsync(tempFilePath);
            var videoStream = videoInfo.PrimaryVideoStream;
            if (videoStream != null)
            {
                width = videoStream.Width;
                height = videoStream.Height;
            }

            // Tạo thumbnail từ video với kích thước tùy chỉnh
            var thumbnailPath = Path.ChangeExtension(tempFilePath, ".jpg"); // Tạo file thumbnail
            var thumbnailTime = TimeSpan.FromSeconds(1); // Thay đổi thời gian theo yêu cầu của bạn (ở giây thứ 1)

            await FFMpegArguments
                .FromFileInput(tempFilePath)
                .OutputToFile(thumbnailPath, overwrite: true, options => options
                    .WithVideoCodec("mjpeg") // Đảm bảo mã hóa thumbnail thành JPEG
                    .WithCustomArgument($"-ss {thumbnailTime.TotalSeconds}") // Tạo thumbnail từ thời gian chỉ định
                    .WithCustomArgument($"-vf scale={thumbnailWidth}:{thumbnailHeight}") // Thay đổi kích thước thumbnail
                )
                .ProcessAsynchronously();

            // Lưu video lên MinIO
            await using (var stream = video.OpenReadStream())
            {
                var filetype = video.ContentType.Split('/').First();
                var extension = video.ContentType.Split('/').Last();
                await minioClientService.StoreObject(
                    MAIN_BUCKET_NAME, stream, GenerateFileName(filetype, extension), video.ContentType);
            }

            // Lưu thumbnail vào MinIO
            await using (var thumbnailStream = new FileStream(thumbnailPath, FileMode.Open, FileAccess.Read))
            {
                var thumbnailFileName = GenerateFileName("thumbnail", "jpg"); // Tạo tên file cho thumbnail
                await minioClientService.StoreObject(MAIN_BUCKET_NAME, thumbnailStream, thumbnailFileName, "image/jpeg");
            }

            // Xóa file tạm sau khi xử lý
            System.IO.File.Delete(tempFilePath);
            System.IO.File.Delete(thumbnailPath);

            return Ok(new
            {
                fileName = video.FileName,
                size = video.Length,
                width,
                height,
                thumbnail = new {
                    width = thumbnailWidth,
                    height = thumbnailHeight
                }
            });
        }

    }
}
