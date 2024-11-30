using InfinityNetServer.BuildingBlocks.Domain.Entities;
using InfinityNetServer.BuildingBlocks.Domain.Specifications;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.BuildingBlocks.Domain.Repositories
{
    public interface ISqlRepository<TEntity, TId> where TEntity : AuditEntity<TId>
    {

        public Task<List<TEntity>> GetAllAsync();

        public Task<TEntity> GetByIdAsync(TId id);

        public Task CreateAsync(IEnumerable<TEntity> items);

        public Task<TEntity> CreateAsync(TEntity item);

        public Task<TEntity> UpdateAsync(TEntity item);

        public Task<TEntity> DeleteAsync(TId id);

        public Task<TEntity> SoftDeleteAsync(TId id);

        public Task<List<TEntity>> FindAsync(Func<TEntity, bool> predicate);

        public Task<bool> ExistsAsync(TId id);

        public Task<int> CountAsync();

        public Task<PagedResult<TEntity>> GetPagedAsync(ISqlSpecification<TEntity> spec);

        public Task<CursorPagedResult<TEntity>> GetPagedAsync(SpecificationWithCursor<TEntity> spec);

    }

}
