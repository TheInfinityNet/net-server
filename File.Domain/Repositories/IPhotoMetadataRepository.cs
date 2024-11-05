using InfinityNetServer.Services.File.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.File.Domain.Repositories
{
    public interface IPhotoMetadataRepository
    {

        Task CreateAsync(PhotoMetadata entity);

        Task UpdateAsync(PhotoMetadata entity);

        Task DeleteAsync(PhotoMetadata entity);

        Task<PhotoMetadata> GetByIdAsync(Guid id);

        Task<IList<PhotoMetadata>> GetAllAsync();

        Task<long> CountAsync();

    }
}
