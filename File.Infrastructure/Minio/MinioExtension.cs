﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;

namespace InfinityNetServer.Services.File.Infrastructure.Minio
{
    public static class MinioExtension
    {

        public static IServiceCollection AddMinioClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(sp =>
            {
                var minioOptions = configuration.GetSection("Minio").Get<MinioOptions>();

                return new MinioClient()
                    .WithEndpoint(minioOptions.Endpoint, minioOptions.Port)
                    .WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey)
                    .Build();
            });

            return services;
        }

    }
}
