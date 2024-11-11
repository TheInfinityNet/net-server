using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.Services.File.Domain.Entities;
using InfinityNetServer.Services.File.Domain.Repositories;
using InfinityNetServer.Services.File.Infrastructure.MongoDb;

namespace InfinityNetServer.Services.File.Infrastructure.Repositories
{
    public class PhotoMetadataRepository(
        FileDbContext dbContext, 
        IAuthenticatedUserService authenticatedUserService) 
        : MongoDbGenericRepository<PhotoMetadata>(dbContext, authenticatedUserService), IPhotoMetadataRepository
    {



    }
}
