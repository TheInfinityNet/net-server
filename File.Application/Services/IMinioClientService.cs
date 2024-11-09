using System.IO;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.File.Application.Services
{
    public interface IMinioClientService
    {

        Task StoreObject(Stream file, string fileName, string contentType, string bucketName);

        Task DeleteObject(string objectKey, string bucketName);

        Task CopyObject(string sourceBucket, string sourceObjectKey, string destinationBucket, string destinationObjectKey);

        Task DeleteAllObjectsInBucket(string bucketName);

    }
}
