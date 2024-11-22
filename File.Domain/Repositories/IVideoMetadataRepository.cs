using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.Services.File.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.File.Domain.Repositories
{
    public interface IVideoMetadataRepository : IMongoDbGenericRepository<VideoMetadata, Guid>
    {

        Task SoftDeleteAsync(VideoMetadata entity);

    }
}
