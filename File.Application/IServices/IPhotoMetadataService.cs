using InfinityNetServer.Services.File.Domain.Entities;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.File.Application.IServices
{
    public interface IPhotoMetadataService
    {

        Task Create(PhotoMetadata photoMetadata);

        Task Update(PhotoMetadata photoMetadata);

        Task Delete(string id);

        Task<PhotoMetadata> GetById(string id);

    }
}
