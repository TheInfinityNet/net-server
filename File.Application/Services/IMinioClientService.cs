using System.IO;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.File.Application.Services
{
    public interface IMinioClientService
    {

        Task StoreObject(Stream file, string fileName, string contentType);

        Task DeleteObject(string objectKey);

    }
}
