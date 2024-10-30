using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.BuildingBlocks.Domain.Specifications;
using InfinityNetServer.BuildingBlocks.Domain.Entities;

namespace InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories
{
    public class SqlRepository<TEntity, TId>(DbContext context) : ISqlRepository<TEntity, TId> where TEntity : AuditEntity
    {

        protected readonly DbSet<TEntity> DbSet = context.Set<TEntity>();

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(TId id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task CreateAsync(IEnumerable<TEntity> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            await DbSet.AddRangeAsync(items);
            await context.SaveChangesAsync();
        }

        public async Task<TEntity> CreateAsync(TEntity item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            await DbSet.AddAsync(item);
            await context.SaveChangesAsync();
            return item;
        }

        public async Task UpdateAsync(TEntity item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            DbSet.Attach(item);
            context.Entry(item).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TId id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException($"Entity with id {id} not found.");
            DbSet.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task<List<TEntity>> FindAsync(Func<TEntity, bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return await Task.FromResult(DbSet.AsEnumerable().Where(predicate).ToList());
        }

        public async Task<bool> ExistsAsync(TId id)
        {
            var entity = await GetByIdAsync(id);
            return entity != null;
        }

        public async Task<int> CountAsync()
        {
            return await DbSet.CountAsync();
        }

        public async Task<PagedResult<TEntity>> GetPagedAsync(ISqlSpecification<TEntity> spec)
        {
            if (spec == null) throw new ArgumentNullException(nameof(spec));

            IQueryable<TEntity> query = ApplySpecification(spec);

            var totalItems = await query.CountAsync();

            var items = await query
                .Skip(spec.Skip ?? 0)
                .Take(spec.Take ?? 10)
                .ToListAsync();

            var pageNumber = spec.Skip.HasValue && spec.Take.HasValue
                ? (spec.Skip.Value / spec.Take.Value) + 1
                : 1;

            return new PagedResult<TEntity>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = spec.Take ?? 10
            };
        }

        private IQueryable<TEntity> ApplySpecification(ISqlSpecification<TEntity> spec)
        {
            IQueryable<TEntity> query = DbSet;

            // Apply Criteria
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            // Apply Includes
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            // Apply Sorting
            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            else if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            return query;
        }

    }

}
