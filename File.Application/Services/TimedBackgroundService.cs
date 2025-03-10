﻿using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using System;
using InfinityNetServer.Services.File.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using static InfinityNetServer.Services.File.Application.Helpers.FileHelper;
using InfinityNetServer.Services.File.Application.IServices;

namespace InfinityNetServer.Services.File.Application.Services
{
    public class TimedBackgroundService
        (ILogger<TimedBackgroundService> logger, IMinioClientService minioClientService) : BackgroundService
    {
        private readonly TimeSpan _interval = TimeSpan.FromDays(1);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Thực thi công việc định kỳ của bạn ở đây
                    logger.LogInformation("Background job running at: {time}", DateTimeOffset.Now);

                    await minioClientService.DeleteAllObjectsInBucket(TEMP_BUCKET_NAME);

                    // Chờ đến lần thực hiện tiếp theo
                    await Task.Delay(_interval, stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    logger.LogInformation("Background job is stopping.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while executing the background job.");
                    throw new FileException(FileError.CAN_NOT_DELETE_FILE, StatusCodes.Status422UnprocessableEntity);
                }
            }
        }
    }
}
