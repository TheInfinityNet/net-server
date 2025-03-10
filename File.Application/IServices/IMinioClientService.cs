﻿using System.IO;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.File.Application.IServices
{
    public interface IMinioClientService
    {

        Task StoreObject(string bucketName, Stream file, string fileName, string contentType);

        Task DeleteObject(string bucketName, string objectKey);

        Task CopyObject(string sourceBucket, string sourceObjectKey, string destinationBucket, string destinationObjectKey);

        Task DeleteAllObjectsInBucket(string bucketName);

        Task<string> GetObjectUrl(string bucketName, string objectKey);

    }
}
