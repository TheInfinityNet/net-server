using InfinityNetServer.Services.File.Application.Services;
using InfinityNetServer.Services.File.Infrastructure.Minio;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;
using System.IO;
using System.Threading.Tasks;
using System;
using InfinityNetServer.Services.File.Application.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace InfinityNetServer.Services.File.Presentation.Services
{
    public class MinioClientService(
        IMinioClient minioClient, 
        IConfiguration configuration, 
        ILogger<MinioClientService> logger) : IMinioClientService
    {

        public string MainBucket { get; } = MinioConnectionExtension.GetMainBucketName(configuration);

        public string TempBucket { get; } = MinioConnectionExtension.GetTempBucketName(configuration);

        public async Task StoreObject(Stream file, string fileName, string contentType, string bucketName)
        {
            try
            {
                // Check if the bucket exists, and create if it doesn't
                await EnsureBucketExists(bucketName);

                // Store object in the bucket
                await minioClient.PutObjectAsync(new PutObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(fileName)
                    .WithStreamData(file)
                    .WithObjectSize(file.Length)
                    .WithContentType(contentType)
                );
            }
            catch (Exception ex)
            {
                logger.LogError($"Error occurred: {ex.Message}");
                throw new FileException(FileErrorCode.CAN_NOT_STORE_FILE, StatusCodes.Status422UnprocessableEntity);
            }
        }

        public async Task DeleteObject(string objectKey, string bucketName)
        {
            try
            {
                await EnsureBucketExists(bucketName);

                // Delete the object from the bucket
                await minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectKey)
                );
            }
            catch (Exception ex)
            {
                logger.LogError($"Error occurred: {ex.Message}");
                throw new FileException(FileErrorCode.CAN_NOT_DELETE_FILE, StatusCodes.Status422UnprocessableEntity);
            }
        }

        private async Task EnsureBucketExists(string bucketName)
        {
            try
            {
                var bucketExists = await minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName));
                if (!bucketExists)
                {
                    await minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName));
                    logger.LogInformation($"Bucket '{bucketName}' created successfully.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Error ensuring bucket '{bucketName}' exists: {ex.Message}");
                throw new FileException(FileErrorCode.BUCKET_CREATION_FAILED, StatusCodes.Status500InternalServerError);
            }
        }

        public async Task CopyObject(string sourceBucket, string sourceObjectKey, string destinationBucket, string destinationObjectKey)
        {
            try
            {
                await EnsureBucketExists(sourceBucket);
                await EnsureBucketExists(destinationBucket);

                await minioClient.CopyObjectAsync(new CopyObjectArgs()
                    .WithBucket(destinationBucket) 
                    .WithObject(destinationObjectKey) // New name for the copied object
                    .WithCopyObjectSource(new CopySourceObjectArgs()
                        .WithBucket(sourceBucket) // Source bucket
                        .WithObject(sourceObjectKey) // Original object key to be copied
                    )
                );

                logger.LogInformation($"Object '{sourceObjectKey}' copied to '{destinationObjectKey}' successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error occurred while copying object: {ex.Message}");
                throw new FileException(FileErrorCode.CAN_NOT_COPY_FILE, StatusCodes.Status422UnprocessableEntity);
            }
        }


    }
}
