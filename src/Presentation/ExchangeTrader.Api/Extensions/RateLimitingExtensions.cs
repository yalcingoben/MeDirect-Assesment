using AspNetCoreRateLimit;
using AspNetCoreRateLimit.Redis;
using ExchangeTrader.Caching.Redis;

namespace ExchangeTrader.Api.Extensions
{
    public static class RateLimitingExtensions
    {
        public static void AddRateLimiting(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<ClientRateLimitOptions>(config.GetSection("ClientRateLimiting"));

            var redisConfiguration = config.GetSection("RedisConfiguration").Get<RedisConfiguration>();
            ArgumentNullException.ThrowIfNull(nameof(redisConfiguration));

            services.AddStackExchangeRedisCache(opt =>
            {
                opt.Configuration = redisConfiguration.Url;
            });

            services.AddDistributedRateLimiting<AsyncKeyLockProcessingStrategy>();
            services.AddDistributedRateLimiting<RedisProcessingStrategy>();
            services.AddRedisRateLimiting();

            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            services.AddSingleton<IClientPolicyStore, DistributedCacheClientPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, DistributedCacheRateLimitCounterStore>();
        }

    }
}
