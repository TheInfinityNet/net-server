using InfinityNetServer.BuildingBlocks.Domain.Entities;
using InfinityNetServer.BuildingBlocks.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.BuildingBlocks.Domain.Repositories
{
    public interface ISqlRepository<TEntity, TId> where TEntity : AuditEntity
    {

        Task<List<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdAsync(TId id);

        Task CreateAsync(IEnumerable<TEntity> items);

        Task<TEntity> CreateAsync(TEntity item);

        Task UpdateAsync(TEntity item);

        Task DeleteAsync(TId id);

        Task<List<TEntity>> FindAsync(Func<TEntity, bool> predicate);

        Task<bool> ExistsAsync(TId id);

        Task<int> CountAsync();

        // Phương thức sử dụng Specification
        Task<PagedResult<TEntity>> GetPagedAsync(ISqlSpecification<TEntity> spec);
        Task<PagedCursorResult<TEntity>> GetPagedCursorAsync(ISqlSpecification<TEntity> spec, int pageSize, Guid? cursor = null);
    }

}
