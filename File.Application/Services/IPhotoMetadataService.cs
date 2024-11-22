using InfinityNetServer.Services.File.Domain.Entities;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.File.Application.Services
{
    public interface IPhotoMetadataService
    {

        Task Create(PhotoMetadata photoMetadata);

        Task Update(PhotoMetadata photoMetadata);

        Task Delete(string id);

        Task<PhotoMetadata> GetById(string id);

    }
}
