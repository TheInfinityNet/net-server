using Castle.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;

namespace InfinityNetServer.Services.File.Infrastructure.Minio
{
    public static class MinioConnectionExtension
    {

        public static IServiceCollection AddMinioClient(this IServiceCollection services, 
            Microsoft.Extensions.Configuration.IConfiguration configuration)
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

        // Helper method to retrieve MinioClient directly (if needed separately)
        public static IMinioClient GetMinioClient(Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            var minioOptions = configuration.GetSection("Minio").Get<MinioOptions>();

            return new MinioClient()
                .WithEndpoint(minioOptions.Endpoint)
                .WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey)
                .Build();
        }

        // Helper method to get bucket name from configuration
        public static string GetBucketName(Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            var minioOptions = configuration.GetSection("Minio").Get<MinioOptions>();
            return minioOptions.BucketName;
        }

    }
}
