using InfinityNetServer.BuildingBlocks.Domain.Entities;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.BuildingBlocks.Domain.Repositories
{
    public interface IMongoDbGenericRepository<TEntity, TId> where TEntity : MongoEntity<TId>
    {

        public Task CreateAsync(TEntity entity);

        public Task UpdateAsync(TEntity entity);

        public Task DeleteAsync(TId id);

        public Task<TEntity> GetByIdAsync(TId id);

        public Task<IList<TEntity>> GetAllAsync();

        public Task<long> CountAsync();

        public Task<CursorPagedResult<TEntity>> GetPagedDataAsync(string cursor, SpecificationWithCursor<TEntity> specification); 

    }
}
