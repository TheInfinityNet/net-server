using InfinityNetServer.BuildingBlocks.Domain.Entities;
using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using Microsoft.EntityFrameworkCore;
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
            => await _collection.Find(e => e.Id.Equals(id)).FirstOrDefaultAsync();

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

            // Apply Cursor-based filter for pagination using created_at
            if (cursor != null)
            {
                var cursorDate = DateTime.Parse(cursor);
                filters.Add(filterBuilder.Gt("created_at", cursorDate)); // Replace with Lt for descending
            }

            var filter = filters.Count > 0 ? filterBuilder.And(filters) : filterBuilder.Empty;

            // Define Sorting based on OrderFields
            var sortBuilder = Builders<TEntity>.Sort;
            SortDefinition<TEntity> sortDefinition;

            if (specification.OrderFields != null && specification.OrderFields.Any())
            {
                var sortDefinitions = new List<SortDefinition<TEntity>>();

                foreach (var orderField in specification.OrderFields)
                {
                    var sort = orderField.Direction.Equals(SortDirection.Ascending)
                        ? sortBuilder.Ascending(orderField.Field)
                        : sortBuilder.Descending(orderField.Field);
                    sortDefinitions.Add(sort);
                }

                sortDefinition = sortBuilder.Combine(sortDefinitions);
            }
            else
            {
                // Default sort: prioritize `is_read` true first, then `created_at` descending
                sortDefinition = sortBuilder.Combine(
                    sortBuilder.Descending("created_at")
                );
            }

            // Execute query with filter and sorting, apply pagination
            var query = _collection.Find(filter)
                .Sort(sortDefinition)
                .Limit(specification.PageSize + 1); // Fetch one extra to check if there's a next page

            var items = await query.ToListAsync();

            // Determine if there's a next page
            var hasNext = items.Count > specification.PageSize;
            if (hasNext) items = items.Take(specification.PageSize).ToList(); // Remove extra item if there is a next page

            // Determine the next and previous cursors
            var nextCursor = hasNext ? items.Last().CreatedAt.ToString("o") : null;
            var prevCursor = items.Count > 1 ? items.First().CreatedAt.ToString("o") : null; // The first item in the current page is the prevCursor

            return new CursorPagedResult<TEntity>
            {
                Items = items,
                NextCursor = nextCursor,
                PrevCursor = prevCursor, // Include prevCursor in the result
            };
        }


    }
}