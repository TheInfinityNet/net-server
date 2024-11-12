using InfinityNetServer.Services.Notification.Domain.Repositories;
using InfinityNetServer.Services.Notification.Infrastructure.MongoDb;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Notification.Infrastructure.Repositories
{
    public class MongoDbGenericRepository<TEntity>(
            NotificationDbContext dbContext) : IMongoDbGenericRepository<TEntity> where TEntity : Domain.Entities.Notification
    {
        protected readonly IMongoCollection<TEntity> _collection = dbContext.GetCollection<TEntity>();

        public virtual async Task CreateAsync(TEntity entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            if (entity.UpdatedAt == null) entity.UpdatedAt = DateTime.Now;
            await _collection.ReplaceOneAsync(e => e.Id == entity.Id, entity, new ReplaceOptions { IsUpsert = false });
        }

        public async Task DeleteAsync(TEntity entity)
        {
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
