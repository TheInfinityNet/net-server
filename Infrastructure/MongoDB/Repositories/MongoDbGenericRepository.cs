using InfinityNetServer.BuildingBlocks.Domain.Entities;
using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.BuildingBlocks.Infrastructure.MongoDB.Repositories
{
    public class MongoDbGenericRepository<TEntity, TId>(
        MongoDbContext dbContext) : IMongoDbGenericRepository<TEntity, TId> where TEntity : MongoEntity<TId>
    {
        protected readonly IMongoCollection<TEntity> _collection = dbContext.GetCollection<TEntity>();

        public virtual async Task CreateAsync(TEntity entity)
        {
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
            => await _collection?.Find(e => e.Id.Equals(id)).FirstOrDefaultAsync();

        public virtual async Task<IList<TEntity>> GetAllAsync()
            => await _collection.Find(_ => true).ToListAsync(); // Directly use Find for better efficiency

        public virtual async Task<long> CountAsync()
            => await _collection.CountDocumentsAsync(e => true);

        public async Task<CursorPagedResult<TEntity>> GetPagedDataAsync(
            string cursor,
            SpecificationWithCursor<TEntity> specification)
        {
            var filterBuilder = Builders<TEntity>.Filter;
            var filters = new List<FilterDefinition<TEntity>>();

            // Apply Criteria filter if specified
            if (specification.Criteria != null)
            {
                var criteriaFilter = new ExpressionFilterDefinition<TEntity>(specification.Criteria);
                filters.Add(criteriaFilter);
            }

            // Apply Cursor-based filter for pagination using CreatedAt and Id
            if (!string.IsNullOrEmpty(cursor))
            {
                var cursorParts = cursor.Split('|');
                if (cursorParts.Length == 2 &&
                    DateTime.TryParse(cursorParts[0], out var cursorDateTime) &&
                    ObjectId.TryParse(cursorParts[1], out var cursorId))
                {
                    var cursorFilter = filterBuilder.Or(
                        filterBuilder.Lt("created_at", cursorDateTime),
                        filterBuilder.And(
                            filterBuilder.Eq("created_at", cursorDateTime),
                            filterBuilder.Lt("_id", cursorId) // Assume MongoDB uses ObjectId as default Id
                        )
                    );
                    filters.Add(cursorFilter);
                }
            }

            var filter = filters.Count > 0 ? filterBuilder.And(filters) : filterBuilder.Empty;

            // Define default sorting: CreatedAt DESC, then Id DESC
            var sortBuilder = Builders<TEntity>.Sort;
            var sortDefinition = sortBuilder.Combine(
                sortBuilder.Descending("created_at"),
                sortBuilder.Descending("_id")
            );

            // Execute query with filter and sorting, apply pagination
            var query = _collection.Find(filter)
                .Sort(sortDefinition)
                .Limit(specification.Limit + 1); // Fetch one extra to check if there's a next page

            var items = await query.ToListAsync();

            // Determine if there's a next page
            var hasNext = items.Count > specification.Limit;
            if (hasNext) items = items.Take(specification.Limit).ToList(); // Remove extra item if there is a next page

            // Determine the next and previous cursors
            var nextCursor = hasNext
                ? $"{items.Last().CreatedAt:O}|{items.Last().Id}"
                : null;

            var prevCursor = !string.IsNullOrEmpty(cursor) && items.Any()
                ? $"{items.First().CreatedAt:O}|{items.First().Id}"
                : null;

            return new CursorPagedResult<TEntity>
            {
                Items = items,
                NextCursor = nextCursor,
                PrevCursor = prevCursor,
            };
        }

    }
}