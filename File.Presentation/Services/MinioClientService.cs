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

        private readonly string _bucketName = MinioConnectionExtension.GetBucketName(configuration);

        public async Task StoreObject(Stream file, string fileName, string contentType)
        {
            try
            {
                // Check if the bucket exists, and create if it doesn't
                await EnsureBucketExists();

                // Store object in the bucket
                await minioClient.PutObjectAsync(new PutObjectArgs()
                    .WithBucket(_bucketName)
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

        public async Task DeleteObject(string objectKey)
        {
            try
            {
                await EnsureBucketExists();

                // Delete the object from the bucket
                await minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(objectKey)
                );
            }
            catch (Exception ex)
            {
                logger.LogError($"Error occurred: {ex.Message}");
                throw new FileException(FileErrorCode.CAN_NOT_DELETE_FILE, StatusCodes.Status422UnprocessableEntity);
            }
        }

        private async Task EnsureBucketExists()
        {
            try
            {
                var bucketExists = await minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(_bucketName));
                if (!bucketExists)
                {
                    await minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucketName));
                    logger.LogInformation($"Bucket '{_bucketName}' created successfully.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Error ensuring bucket '{_bucketName}' exists: {ex.Message}");
                throw new FileException(FileErrorCode.BUCKET_CREATION_FAILED, StatusCodes.Status500InternalServerError);
            }
        }

        public async Task CopyObject(string sourceObjectKey, string destinationObjectKey)
        {
            try
            {
                await EnsureBucketExists();

                // Copy object within the same bucket with a new object key
                await minioClient.CopyObjectAsync(new CopyObjectArgs()
                    .WithBucket(_bucketName) // Source and destination bucket are the same
                    .WithObject(destinationObjectKey) // New name for the copied object
                    .WithCopyObjectSource(new CopySourceObjectArgs()
                        .WithBucket(_bucketName) // Source bucket
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
