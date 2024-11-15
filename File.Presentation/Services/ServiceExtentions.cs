﻿using InfinityNetServer.Services.File.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InfinityNetServer.Services.File.Presentation.Services
{
    public static class ServiceExtentions
    {

        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IMinioClientService, MinioClientService>();
            services.AddHostedService<TimedBackgroundService>();
            services.AddScoped<IPhotoMetadataService, PhotoMetadataService>();
            services.AddScoped<IVideoMetadataService, VideoMetadataService>();
        }

    }

}
