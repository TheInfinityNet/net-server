using InfinityNetServer.BuildingBlocks.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.BuildingBlocks.Domain.Repositories
{
    public interface IMongoDbGenericRepository<TEntity, TId> where TEntity : MongoEntity<TId>
    {

        Task CreateAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(TId id);

        Task<TEntity> GetByIdAsync(TId id);

        Task<IList<TEntity>> GetAllAsync();

        Task<long> CountAsync();

    }
}
