using InfinityNetServer.BuildingBlocks.Application.IServices;
using InfinityNetServer.BuildingBlocks.Infrastructure.MongoDB.Repositories;
using InfinityNetServer.Services.File.Domain.Entities;
using InfinityNetServer.Services.File.Domain.Repositories;
using InfinityNetServer.Services.File.Infrastructure.Data;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.File.Infrastructure.Repositories
{
    public class VideoMetadataRepository(
        FileDbContext dbContext,
        IAuthenticatedUserService authenticatedUserService)
        : MongoDbGenericRepository<VideoMetadata, Guid>(dbContext), IVideoMetadataRepository
    {

        public override async Task CreateAsync(VideoMetadata entity)
        {
            if (entity.CreatedBy == null) entity.CreatedBy = authenticatedUserService.GetAuthenticatedProfileId();
            await base.CreateAsync(entity);
        }

        public override async Task UpdateAsync(VideoMetadata entity)
        {
            if (entity.UpdatedBy == null) entity.UpdatedBy = authenticatedUserService.GetAuthenticatedProfileId();
            await base.UpdateAsync(entity);
        }

        public async Task SoftDeleteAsync(VideoMetadata entity)
        {
            if (entity.DeletedBy == null) entity.DeletedBy = authenticatedUserService.GetAuthenticatedProfileId();
            if (entity.DeletedAt == null) entity.DeletedAt = DateTime.Now;
            entity.IsDeleted = true;
            await _collection.ReplaceOneAsync(e => e.Id == entity.Id, entity, new ReplaceOptions { IsUpsert = false });
        }

    }
}
