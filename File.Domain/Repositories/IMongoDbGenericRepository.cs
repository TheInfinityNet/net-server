using InfinityNetServer.Services.File.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.File.Domain.Repositories
{
    public interface IMongoDbGenericRepository<TEntity> where TEntity : BaseMetadata
    {
        Task CreateAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);

        Task<TEntity> GetByIdAsync(Guid id);

        Task<IList<TEntity>> GetAllAsync();

        Task<long> CountAsync();

    }
}
