using ExchangeTrader.App.Abstractions.Caching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace ExchangeTrader.Caching.Redis.DependencyInjection
{
    public static class ServiceRegistrationExtensions
    {
        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(nameof(services));
            ArgumentNullException.ThrowIfNull(nameof(configuration));

            var redisConfiguration = configuration.GetSection("RedisConfiguration").Get<RedisConfiguration>();
            ArgumentNullException.ThrowIfNull(nameof(redisConfiguration));

            var multiplexer = ConnectionMultiplexer.Connect(new ConfigurationOptions
            {
                EndPoints = { $"{redisConfiguration.Url}" }
            });
            services.AddSingleton<IConnectionMultiplexer>(multiplexer);
            services.AddSingleton<ICacheService, RedisCacheService>();
            return services;
        }
    }
}
