using InfinityNetServer.Services.File.Application.Services;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;
using System.IO;
using System.Threading.Tasks;
using System;
using InfinityNetServer.Services.File.Application.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using InfinityNetServer.Services.File.Domain.Repositories;

namespace InfinityNetServer.Services.File.Presentation.Services
{
    public class MinioClientService(
        IMinioClient minioClient, 
        IConfiguration configuration, 
        ILogger<MinioClientService> logger,
        IPhotoMetadataRepository fileMetadataRepository) : IMinioClientService
    {

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

        public async Task DeleteAllObjectsInBucket(string bucketName)
        {
            try
            {
                // Kiểm tra xem bucket có tồn tại không
                await EnsureBucketExists(bucketName);

                // Sử dụng ListObjects để liệt kê các object trong bucket
                var listArgs = new ListObjectsArgs()
                    .WithBucket(bucketName)
                    .WithRecursive(true); // Lấy toàn bộ object, bao gồm cả các object trong thư mục con (nếu có)

                var objectEnumerator = minioClient.ListObjectsEnumAsync(listArgs).GetAsyncEnumerator();

                // Lặp qua từng object và xóa nó
                while (await objectEnumerator.MoveNextAsync())
                {

                    var obj = objectEnumerator.Current;
                    await minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                        .WithBucket(bucketName)
                        .WithObject(obj.Key)
                    );

                }

                logger.LogInformation($"All objects in bucket '{bucketName}' have been deleted successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting all objects in bucket '{bucketName}': {ex.Message}");
                throw new FileException(FileErrorCode.CAN_NOT_DELETE_FILE, StatusCodes.Status422UnprocessableEntity);
            }
        }
        
    }
}
