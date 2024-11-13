using InfinityNetServer.BuildingBlocks.Domain.Entities;
using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.BuildingBlocks.Infrastructure.MongoDB.Repositories
{
    public class MongoDbGenericRepository<TEntity, TId>(
        MongoDbContext dbContext) : IMongoDbGenericRepository<TEntity, TId> where TEntity : MongoEntity<TId>
    {
        protected readonly IMongoCollection<TEntity> _collection = dbContext.GetCollection<TEntity>();

        public virtual async Task CreateAsync(TEntity entity)
        {
            if (entity.CreatedAt == null) entity.CreatedAt = DateTime.Now;
            await _collection.InsertOneAsync(entity);
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            if (entity.UpdatedAt == null) entity.UpdatedAt = DateTime.Now; // Always set updated time
            await _collection.ReplaceOneAsync(e => e.Id.Equals(entity.Id), entity, new ReplaceOptions { IsUpsert = false });
        }

        public async Task DeleteAsync(TId id)
            => await _collection.DeleteOneAsync(e => e.Id.Equals(id)); // Use DeleteOneAsync for deletion

        public virtual async Task<TEntity> GetByIdAsync(TId id)
            => await _collection.Find(e => e.Id.Equals(id)).FirstOrDefaultAsync();

        public virtual async Task<IList<TEntity>> GetAllAsync()
            => await _collection.Find(_ => true).ToListAsync(); // Directly use Find for better efficiency

        public virtual async Task<long> CountAsync()
            => await _collection.CountDocumentsAsync(e => true);
    }


}
