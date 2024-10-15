using StackExchange.Redis;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using InfinityNetServer.BuildingBlocks.Application.Services;

namespace InfinityNetServer.BuildingBlocks.Presentation.Services
{
    public class BaseRedisService<TKey, TValue> : IBaseRedisService<TKey, TValue>
    {
        private readonly IDatabase _database;

        public BaseRedisService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task SetAsync(TKey key, TValue value)
        {
            await _database.StringSetAsync(key?.ToString(), value?.ToString());
        }

        public async Task SetWithExpirationAsync(TKey key, TValue value, TimeSpan expiry)
        {
            await _database.StringSetAsync(key?.ToString(), value?.ToString(), expiry);
        }

        public async Task<TValue> GetAsync(TKey key)
        {
            var value = await _database.StringGetAsync(key.ToString());
            return (TValue)Convert.ChangeType(value, typeof(TValue));
        }

        public async Task<bool> ExistsAsync(TKey key)
        {
            return await _database.KeyExistsAsync(key?.ToString());
        }

        public async Task<bool> DeleteAsync(TKey key)
        {
            return await _database.KeyDeleteAsync(key?.ToString());
        }

        public async Task<bool> SetExpireAsync(TKey key, TimeSpan expiry)
        {
            return await _database.KeyExpireAsync(key?.ToString(), expiry);
        }

        public async Task HashSetAsync<TField>(TKey key, TField field, TValue value)
        {
            await _database.HashSetAsync(key?.ToString(), field?.ToString(), value?.ToString());
        }

        public async Task<TValue> HashGetAsync<TField>(TKey key, TField field)
        {
            var value = await _database.HashGetAsync(key?.ToString(), field?.ToString());
            return (TValue)Convert.ChangeType(value, typeof(TValue));
        }

        public async Task<long> HashDeleteAsync<TField>(TKey key, params TField[] fields)
        {
            var redisFields = Array.ConvertAll(fields, field => (RedisValue)field?.ToString());
            return await _database.HashDeleteAsync(key?.ToString(), redisFields);
        }

        public async Task<long> IncrementAsync<TField>(TKey key, TField field, long delta)
        {
            return await _database.HashIncrementAsync(key?.ToString(), field?.ToString(), delta);
        }

        public async Task<Dictionary<TField, TValue>> HashEntriesAsync<TField>(TKey key)
        {
            var entries = await _database.HashGetAllAsync(key?.ToString());
            var result = new Dictionary<TField, TValue>();

            foreach (var entry in entries)
            {
                var field = (TField)Convert.ChangeType(entry.Name, typeof(TField));
                var value = (TValue)Convert.ChangeType(entry.Value, typeof(TValue));
                result.Add(field, value);
            }

            return result;
        }

    }
}
