using InfinityNetServer.Services.File.Application.IServices;
using InfinityNetServer.Services.File.Application.Services;
using InfinityNetServer.Services.File.Infrastructure.Minio;
using Microsoft.Extensions.DependencyInjection;

namespace InfinityNetServer.Services.File.Presentation.Configurations
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
