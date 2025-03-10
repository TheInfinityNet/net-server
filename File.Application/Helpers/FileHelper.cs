﻿using FFMpegCore;
using InfinityNetServer.Services.File.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using SixLabors.ImageSharp;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.File.Application.Helpers
{
    public static class FileHelper
    {

        public static readonly string FAKE_PHOTOS_FOLDER_PATH = Path.Combine(Directory.GetCurrentDirectory(), "FakeData\\Photos").Replace("\\bin\\Debug\\net8.0", "");
        
        public static readonly string FAKE_VIDEOS_FOLDER_PATH = Path.Combine(Directory.GetCurrentDirectory(), "FakeData\\Videos").Replace("\\bin\\Debug\\net8.0", "");
        
        public static readonly string MAIN_BUCKET_NAME = "infinity-net-bucket";

        public static readonly string TEMP_BUCKET_NAME = "infinity-net-temp-bucket";

        public static async Task<(int width, int height)> GetImageDimensionsAsync(Stream imageStream)
        {
            // Reset stream position to the beginning in case it was read before
            imageStream.Position = 0;

            // Load image and get its dimensions
            using var image = await Image.LoadAsync(imageStream);
            return (image.Width, image.Height);
        }

        public static async Task<(int Width, int Height, TimeSpan Duration)> GetVideoInfoAsync(string videoPath)
        {
            var videoInfo = await FFProbe.AnalyseAsync(videoPath);
            var videoStream = videoInfo.PrimaryVideoStream ?? throw new FileException(FileError.FILE_EMPTY, StatusCodes.Status400BadRequest);

            return (videoStream.Width, videoStream.Height, videoStream.Duration);
        }

        public static async Task<string> CreateThumbnail(string videoPath, int width, int height, TimeSpan thumbnailTime)
        {
            var thumbnailPath = Path.ChangeExtension(videoPath, ".jpg");

            await FFMpegArguments
                .FromFileInput(videoPath)
                .OutputToFile(thumbnailPath, overwrite: true, options => options
                    .WithVideoCodec("mjpeg")
                    .WithCustomArgument($"-ss {thumbnailTime.TotalSeconds}")
                    .WithCustomArgument($"-vf scale={width}:{height}")
                    .WithCustomArgument("-frames:v 1")
                )
                .ProcessAsynchronously();

            return thumbnailPath;
        }


        public static void ValidateVideo(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new FileException(FileError.FILE_EMPTY, StatusCodes.Status400BadRequest);

            if (file.Length > 20971520) // 20MB = 20 * 1024 * 1024
                throw new FileException(FileError.FILE_SIZE_EXCEEDED, StatusCodes.Status400BadRequest);

            var allowedExtensions = new[] { ".mp4", ".avi", ".mov", ".mkv" };
            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
                throw new FileException(FileError.INVALID_FILE_TYPE, StatusCodes.Status415UnsupportedMediaType);
        }

        public static void ValidateImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new FileException(FileError.FILE_EMPTY, StatusCodes.Status400BadRequest);

            if (file.Length > 20971520) // 20MB = 20 * 1024 * 1024
                throw new FileException(FileError.FILE_SIZE_EXCEEDED, StatusCodes.Status400BadRequest);

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
                throw new FileException(FileError.INVALID_FILE_TYPE, StatusCodes.Status415UnsupportedMediaType);
        }

        public static void ValidateAudio(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new FileException(FileError.FILE_EMPTY, StatusCodes.Status400BadRequest);

            if (file.Length > 20971520) // 20MB = 20 * 1024 * 1024
                throw new FileException(FileError.FILE_SIZE_EXCEEDED, StatusCodes.Status400BadRequest);

            var allowedExtensions = new[] { ".mp3", ".wav", ".flac", ".aac" };
            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
                throw new FileException(FileError.INVALID_FILE_TYPE, StatusCodes.Status415UnsupportedMediaType);
        }

        public static void ValidateFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new FileException(FileError.FILE_EMPTY, StatusCodes.Status400BadRequest);

            if (file.Length > 20971520) // 20MB = 20 * 1024 * 1024
                throw new FileException(FileError.FILE_SIZE_EXCEEDED, StatusCodes.Status400BadRequest);

            var allowedExtensions = new[] { ".pdf", ".doc", ".docx", ".txt", ".zip", ".rar" }; // Add other file extensions if needed
            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
                throw new FileException(FileError.INVALID_FILE_TYPE, StatusCodes.Status415UnsupportedMediaType);
        }


        public static string GenerateFileName(string fileType, string extension)
        {
            // Lấy thời gian hiện tại với format yyyyMMdd_HHmmss
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);

            // Tạo UUID duy nhất
            string uniqueId = Guid.NewGuid().ToString();

            // Kết hợp tên file
            return $"{fileType}_{timestamp}_{uniqueId}.{extension}";
        }

        public static string GetContentType(string filePath)
            => new FileExtensionContentTypeProvider()
            .TryGetContentType(filePath, out string contentType) ? contentType : "application/octet-stream";

    }
}
