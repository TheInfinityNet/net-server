using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.Services.File.Domain.Entities;
using InfinityNetServer.Services.File.Domain.Repositories;
using InfinityNetServer.Services.File.Infrastructure.MongoDb;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.File.Infrastructure.Repositories
{
    public class PhotoMetadataRepository(
        IAuthenticatedUserService authenticatedUserService, FileDbContext dbContext, string collectionName = "") : IPhotoMetadataRepository
    {

        private readonly IMongoCollection<PhotoMetadata> _collection = dbContext.GetCollection<PhotoMetadata>(collectionName);

        public virtual async Task CreateAsync(PhotoMetadata entity)
        { 
            if (entity.CreatedBy == null) entity.CreatedBy = authenticatedUserService.GetAuthenticatedUserId();
            await _collection.InsertOneAsync(entity); 
        }
        

        public virtual async Task UpdateAsync(PhotoMetadata entity)
        {
            if (entity.UpdatedBy == null) entity.UpdatedBy = authenticatedUserService.GetAuthenticatedUserId();
            if (entity.UpdatedAt == null) entity.UpdatedAt = DateTime.Now;
            await _collection.ReplaceOneAsync(p => p.Id == entity.Id, entity, new ReplaceOptions() { IsUpsert = false });
        }


        public async Task DeleteAsync(PhotoMetadata entity)
        {
            if (entity.DeletedBy == null) entity.DeletedBy = authenticatedUserService.GetAuthenticatedUserId();
            if (entity.DeletedAt == null) entity.DeletedAt = DateTime.Now;
            entity.IsDeleted = true;
            await _collection.ReplaceOneAsync(p => p.Id == entity.Id, entity, new ReplaceOptions() { IsUpsert = false });
        }
        

        public virtual async Task<PhotoMetadata> GetByIdAsync(string id)
            => await _collection.Find(e => e.Id.ToString() == id).FirstOrDefaultAsync();
        

        public virtual async Task<IList<PhotoMetadata>> GetAllAsync()
            => await _collection.AsQueryable().ToListAsync();
        

        public virtual async Task<long> CountAsync()
            => await _collection.CountDocumentsAsync(f => true);

    }
}
