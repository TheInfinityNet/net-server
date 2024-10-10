using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.BuildingBlocks.Application.Interfaces
{
    public interface IBaseRedisService<TKey, TValue>
    {

        Task SetAsync(TKey key, TValue value);

        Task SetWithExpirationAsync(TKey key, TValue value, TimeSpan expiry);

        Task<TValue> GetAsync(TKey key);

        Task<bool> ExistsAsync(TKey key);

        Task<bool> DeleteAsync(TKey key);

        Task<bool> SetExpireAsync(TKey key, TimeSpan expiry);

        Task HashSetAsync<TField>(TKey key, TField field, TValue value);

        Task<TValue> HashGetAsync<TField>(TKey key, TField field);

        Task<long> HashDeleteAsync<TField>(TKey key, params TField[] fields);

        Task<long> IncrementAsync<TField>(TKey key, TField field, long delta);

        Task<Dictionary<TField, TValue>> HashEntriesAsync<TField>(TKey key);

    }
}
