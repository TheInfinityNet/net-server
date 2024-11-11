using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.Services.File.Domain.Entities;
using InfinityNetServer.Services.File.Domain.Repositories;
using InfinityNetServer.Services.File.Infrastructure.MongoDb;

namespace InfinityNetServer.Services.File.Infrastructure.Repositories
{
    public class VideoMetadataRepository(
        FileDbContext dbContext, 
        IAuthenticatedUserService authenticatedUserService) 
        : MongoDbGenericRepository<VideoMetadata>(dbContext, authenticatedUserService), IVideoMetadataRepository
    {



    }
}
