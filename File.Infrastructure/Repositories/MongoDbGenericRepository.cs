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
    public class MongoDbGenericRepository<TEntity>(
            FileDbContext dbContext,
            IAuthenticatedUserService authenticatedUserService) : IMongoDbGenericRepository<TEntity> where TEntity : BaseMetadata
    {
        protected readonly IMongoCollection<TEntity> _collection = dbContext.GetCollection<TEntity>();

        public virtual async Task CreateAsync(TEntity entity)
        {
            if (entity.CreatedBy == null) entity.CreatedBy = authenticatedUserService.GetAuthenticatedUserId();
            await _collection.InsertOneAsync(entity);
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            if (entity.UpdatedBy == null) entity.UpdatedBy = authenticatedUserService.GetAuthenticatedUserId();
            if (entity.UpdatedAt == null) entity.UpdatedAt = DateTime.Now;
            await _collection.ReplaceOneAsync(e => e.Id == entity.Id, entity, new ReplaceOptions { IsUpsert = false });
        }

        public async Task DeleteAsync(TEntity entity)
        {
            if (entity.DeletedBy == null) entity.DeletedBy = authenticatedUserService.GetAuthenticatedUserId();
            if (entity.DeletedAt == null) entity.DeletedAt = DateTime.Now;
            entity.IsDeleted = true;
            await _collection.ReplaceOneAsync(e => e.Id == entity.Id, entity, new ReplaceOptions { IsUpsert = false });
        }

        public virtual async Task<TEntity> GetByIdAsync(Guid id)
            => await _collection.Find(e => e.Id == id).FirstOrDefaultAsync();

        public virtual async Task<IList<TEntity>> GetAllAsync()
            => await _collection.AsQueryable().ToListAsync();

        public virtual async Task<long> CountAsync()
            => await _collection.CountDocumentsAsync(e => true);
    }

}
