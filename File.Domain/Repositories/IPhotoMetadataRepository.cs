using InfinityNetServer.Services.File.Domain.Entities;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.File.Domain.Repositories
{
    public interface IPhotoMetadataRepository
    {

        Task CreateAsync(PhotoMetadata entity);

        Task UpdateAsync(PhotoMetadata entity);

        Task DeleteAsync(ObjectId id);

        Task<PhotoMetadata> GetByIdAsync(ObjectId id);

        Task<IList<PhotoMetadata>> GetAllAsync();

        Task<long> CountAsync();

    }
}
