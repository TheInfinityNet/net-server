using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Infrastructure.MongoDB.Repositories;
using InfinityNetServer.Services.File.Domain.Entities;
using InfinityNetServer.Services.File.Domain.Repositories;
using InfinityNetServer.Services.File.Infrastructure.Data;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.File.Infrastructure.Repositories
{
    public class PhotoMetadataRepository(
        FileDbContext dbContext, 
        IAuthenticatedUserService authenticatedUserService) 
        : MongoDbGenericRepository<PhotoMetadata, Guid>(dbContext), IPhotoMetadataRepository
    {

        public override async Task CreateAsync(PhotoMetadata entity)
        {
            if (entity.CreatedBy == null) entity.CreatedBy = authenticatedUserService.GetAuthenticatedUserId();
            await base.CreateAsync(entity);
        }

        public override async Task UpdateAsync(PhotoMetadata entity)
        {
            if (entity.UpdatedBy == null) entity.UpdatedBy = authenticatedUserService.GetAuthenticatedUserId();
            await base.UpdateAsync(entity);
        }

        public async Task SoftDeleteAsync(PhotoMetadata entity)
        {
            if (entity.DeletedBy == null) entity.DeletedBy = authenticatedUserService.GetAuthenticatedUserId();
            if (entity.DeletedAt == null) entity.DeletedAt = DateTime.Now;
            entity.IsDeleted = true;
            await _collection.ReplaceOneAsync(e => e.Id == entity.Id, entity, new ReplaceOptions { IsUpsert = false });
        }

    }
}
