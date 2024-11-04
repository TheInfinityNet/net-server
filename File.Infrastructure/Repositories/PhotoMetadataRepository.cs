using InfinityNetServer.Services.File.Domain.Entities;
using InfinityNetServer.Services.File.Domain.Repositories;
using InfinityNetServer.Services.File.Infrastructure.MongoDb;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.File.Infrastructure.Repositories
{
    public class PhotoMetadataRepository(FileDbContext dbContext, string collectionName = "") : IPhotoMetadataRepository
    {

        private readonly IMongoCollection<PhotoMetadata> _collection = dbContext.GetCollection<PhotoMetadata>(collectionName);

        public virtual async Task CreateAsync(PhotoMetadata entity)
            => await _collection.InsertOneAsync(entity);
        

        public virtual async Task UpdateAsync(PhotoMetadata entity)
            => await _collection.ReplaceOneAsync(p => p.Id == entity.Id, entity, new ReplaceOptions() { IsUpsert = false });
        

        public async Task DeleteAsync(ObjectId id)
            => await _collection.DeleteOneAsync(p => p.Id == id);
        

        public virtual async Task<PhotoMetadata> GetByIdAsync(ObjectId id)
            => await _collection.Find(e => e.Id == id).FirstOrDefaultAsync();
        

        public virtual async Task<IList<PhotoMetadata>> GetAllAsync()
            => await _collection.AsQueryable().ToListAsync();
        

        public virtual async Task<long> CountAsync()
            => await _collection.CountDocumentsAsync(f => true);

    }
}
