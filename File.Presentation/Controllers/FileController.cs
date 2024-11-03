using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System;
using Bogus;
using InfinityNetServer.Services.File.Domain.Entities;
using InfinityNetServer.Services.File.Application;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Commands;
using InfinityNetServer.BuildingBlocks.Presentation.Controllers;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.Services.File.Domain.Repositories;
using InfinityNetServer.BuildingBlocks.Application.Bus;
using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.Services.File.Application.DTOs;
using InfinityNetServer.Services.File.Application.Exceptions;
using InfinityNetServer.Services.File.Application.Services;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.IO;
using FFMpegCore;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;

namespace InfinityNetServer.Services.File.Presentation.Controllers
{
    [ApiController]
    public class FileController(
        IAuthenticatedUserService authenticatedUserService,
        ILogger<FileController> logger,
        IStringLocalizer<FileSharedResource> Localizer,
        IMessageBus messageBus,
        IFileMetadataRepository fileMetadataRepository,
        CommonPostClient postClient,
        IMinioClientService minioClientService) : BaseApiController(authenticatedUserService) 
    {

        [HttpGet("test/{type}")]
        public async Task<IActionResult> Test(string type)
        {

            var fileMetadataIdsWithTypes = await postClient.GetFileMetadataIdsWithTypes(type);

            await minioClientService.CopyObject("photo.jpg", GenerateFileName("image", "jpg"));

            return Ok(new
            {
                fileMetadataIdsWithTypes
            });
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadRawFile(IFormFile file)
        {
            ValidateFile(file);

            await using var stream = file.OpenReadStream();
            var filetype = file.ContentType.Split('/').First();
            var extension = file.FileName.Split('.').Last();
            await minioClientService.StoreObject(stream, GenerateFileName(filetype, extension), file.ContentType);

            return Ok(new
            {
                fileName = file.FileName,
                size = file.Length,
                mimeType = file.ContentType
            });
        }

        [HttpPost("upload/photo")]
        public async Task<IActionResult> UploadRawImage(IFormFile photo)
        {
            ValidateImage(photo);

            int width;
            int height;
            await using (var stream = photo.OpenReadStream())
            {
                using (var image = await SixLabors.ImageSharp.Image.LoadAsync(stream))
                {
                    width = image.Width;
                    height = image.Height;
                }

                // Reset stream position to the beginning
                stream.Position = 0;

                var filetype = photo.ContentType.Split('/').First();
                var extension = photo.ContentType.Split('/').Last();
                await minioClientService.StoreObject(stream, GenerateFileName(filetype, extension), photo.ContentType);
            }

            return Ok(new
            {
                fileName = photo.FileName,
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
            {
                await video.CopyToAsync(stream);
            }

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
                await minioClientService.StoreObject(stream, GenerateFileName(filetype, extension), video.ContentType);
            }

            // Lưu thumbnail vào MinIO
            await using (var thumbnailStream = new FileStream(thumbnailPath, FileMode.Open, FileAccess.Read))
            {
                var thumbnailFileName = GenerateFileName("thumbnail", "jpg"); // Tạo tên file cho thumbnail
                await minioClientService.StoreObject(thumbnailStream, thumbnailFileName, "image/jpeg");
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

        private void ValidateVideo(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new FileException(FileErrorCode.FILE_EMPTY, StatusCodes.Status400BadRequest);

            if (file.Length > 20971520) // 20MB = 20 * 1024 * 1024
                throw new FileException(FileErrorCode.FILE_SIZE_EXCEEDED, StatusCodes.Status400BadRequest);

            var allowedExtensions = new[] { ".mp4", ".avi", ".mov", ".mkv" };
            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
                throw new FileException(FileErrorCode.INVALID_FILE_TYPE, StatusCodes.Status415UnsupportedMediaType);
        }

        private void ValidateImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new FileException(FileErrorCode.FILE_EMPTY, StatusCodes.Status400BadRequest);

            if (file.Length > 20971520) // 20MB = 20 * 1024 * 1024
                throw new FileException(FileErrorCode.FILE_SIZE_EXCEEDED, StatusCodes.Status400BadRequest);

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
                throw new FileException(FileErrorCode.INVALID_FILE_TYPE, StatusCodes.Status415UnsupportedMediaType);
        }

        private void ValidateAudio(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new FileException(FileErrorCode.FILE_EMPTY, StatusCodes.Status400BadRequest);

            if (file.Length > 20971520) // 20MB = 20 * 1024 * 1024
                throw new FileException(FileErrorCode.FILE_SIZE_EXCEEDED, StatusCodes.Status400BadRequest);

            var allowedExtensions = new[] { ".mp3", ".wav", ".flac", ".aac" };
            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
                throw new FileException(FileErrorCode.INVALID_FILE_TYPE, StatusCodes.Status415UnsupportedMediaType);
        }

        private void ValidateFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new FileException(FileErrorCode.FILE_EMPTY, StatusCodes.Status400BadRequest);

            if (file.Length > 20971520) // 20MB = 20 * 1024 * 1024
                throw new FileException(FileErrorCode.FILE_SIZE_EXCEEDED, StatusCodes.Status400BadRequest);

            var allowedExtensions = new[] { ".pdf", ".doc", ".docx", ".txt", ".zip", ".rar" }; // Add other file extensions if needed
            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
                throw new FileException(FileErrorCode.INVALID_FILE_TYPE, StatusCodes.Status415UnsupportedMediaType);
        }


        private string GenerateFileName(string fileType, string extension)
        {
            // Lấy thời gian hiện tại với format yyyyMMdd_HHmmss
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);

            // Tạo UUID duy nhất
            string uniqueId = Guid.NewGuid().ToString();

            // Kết hợp tên file
            return $"{fileType}_{timestamp}_{uniqueId}.{extension}";
        }

    }
}
