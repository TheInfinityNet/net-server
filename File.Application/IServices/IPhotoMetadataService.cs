using InfinityNetServer.Services.File.Domain.Entities;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.File.Application.IServices
{
    public interface IPhotoMetadataService
    {

        public Task Create(PhotoMetadata photoMetadata);

        public Task Update(PhotoMetadata photoMetadata);

        public Task SoftDelete(string id);

        public Task Delete(string id);

        public Task<PhotoMetadata> GetById(string id);

    }
}
