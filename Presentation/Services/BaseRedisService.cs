using StackExchange.Redis;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using InfinityNetServer.BuildingBlocks.Application.Services;
using Newtonsoft.Json;

namespace InfinityNetServer.BuildingBlocks.Presentation.Services
{
    public class BaseRedisService<TKey, TValue>(IConnectionMultiplexer redis) : IBaseRedisService<TKey, TValue>
    {
        private readonly IDatabase _database = redis.GetDatabase();

        public async Task SetAsync(TKey key, TValue value)
        {
            var jsonValue = JsonConvert.SerializeObject(value);
            await _database.StringSetAsync(key?.ToString(), jsonValue);
        }


        public async Task SetWithExpirationAsync(TKey key, TValue value, TimeSpan expiry)
        {
            var jsonValue = JsonConvert.SerializeObject(value);
            await _database.StringSetAsync(key?.ToString(), jsonValue, expiry);
        }


        public async Task<TValue> GetAsync(TKey key)
        {
            var value = await _database.StringGetAsync(key.ToString());
            return value.IsNullOrEmpty ? default : JsonConvert.DeserializeObject<TValue>(value);
        }

        public async Task<bool> ExistsAsync(TKey key)
            => await _database.KeyExistsAsync(key?.ToString());
        

        public async Task<bool> DeleteAsync(TKey key)
            => await _database.KeyDeleteAsync(key?.ToString());

        public async Task<bool> SetExpireAsync(TKey key, TimeSpan expiry)
            => await _database.KeyExpireAsync(key?.ToString(), expiry);


        public async Task HashSetAsync<TField>(TKey key, TField field, TValue value)
        {
            var jsonValue = JsonConvert.SerializeObject(value);
            await _database.HashSetAsync(key?.ToString(), field?.ToString(), jsonValue);
        }

        public async Task<TValue> HashGetAsync<TField>(TKey key, TField field)
        {
            var value = await _database.HashGetAsync(key?.ToString(), field?.ToString());
            return value.IsNullOrEmpty ? default : JsonConvert.DeserializeObject<TValue>(value);
        }

        public async Task<long> HashDeleteAsync<TField>(TKey key, params TField[] fields)
        {
            var redisFields = Array.ConvertAll(fields, field => (RedisValue)field?.ToString());
            return await _database.HashDeleteAsync(key?.ToString(), redisFields);
        }

        public async Task<long> IncrementAsync<TField>(TKey key, TField field, long delta)
            => await _database.HashIncrementAsync(key?.ToString(), field?.ToString(), delta);


        public async Task<Dictionary<TField, TValue>> HashEntriesAsync<TField>(TKey key)
        {
            var entries = await _database.HashGetAllAsync(key?.ToString());
            var result = new Dictionary<TField, TValue>();

            foreach (var entry in entries)
            {
                var field = (TField)Convert.ChangeType(entry.Name, typeof(TField));
                var value = JsonConvert.DeserializeObject<TValue>(entry.Value);
                result.Add(field, value);
            }

            return result;
        }

    }
}
