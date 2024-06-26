﻿using ExchangeTrader.App.Abstractions.Caching;
using StackExchange.Redis;
using System.Text.Json;

namespace ExchangeTrader.Caching.Redis
{
    public class RedisCacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _redisCon;
        private readonly IDatabase _cache;

        public RedisCacheService(IConnectionMultiplexer redisCon)
        {
            _redisCon = redisCon;
            _cache = redisCon.GetDatabase();
        }

        public async Task Clear(string key, CancellationToken cancellationToken)
        {
            await _cache.KeyDeleteAsync(key);
        }

        public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> action, TimeSpan? expireTime, CancellationToken cancellationToken) where T : class
        {
            var result = await _cache.StringGetAsync(key);
            if (result.IsNull)
            {
                result = JsonSerializer.SerializeToUtf8Bytes(await action());
                await SetValueAsync(key, result, expireTime, cancellationToken);
            }
            return JsonSerializer.Deserialize<T>(result);
        }

        public async Task<string> GetValueAsync(string key, CancellationToken cancellationToken)
        {
            return await _cache.StringGetAsync(key);
        }

        public async Task<bool> SetValueAsync(string key, string value, TimeSpan? expireTime, CancellationToken cancellationToken)
        {
            return await _cache.StringSetAsync(key, value, expireTime);
        }

        public T GetOrAdd<T>(string key, Func<T> action, TimeSpan? expireTime, CancellationToken cancellationToken) where T : class
        {
            var result = _cache.StringGet(key);
            if (result.IsNull)
            {
                result = JsonSerializer.SerializeToUtf8Bytes(action());
                _cache.StringSet(key, result, expireTime);
            }
            return JsonSerializer.Deserialize<T>(result);
        }
    }
}
