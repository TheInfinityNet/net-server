using InfinityNetServer.BuildingBlocks.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace InfinityNetServer.BuildingBlocks.Infrastructure.Redis
{
    public static class RedisConnectionExtentions
    {

        public static void AddRedisConnection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(GetRedisConfigurationOptions(configuration)
            ));
        }

        private static ConfigurationOptions GetRedisConfigurationOptions(IConfiguration configuration)
        {
            return new ConfigurationOptions
            {
                EndPoints = { configuration["Redis:Connection"] },
                //Password = configuration["Redis:Password"]!,
                Ssl = false
            };
        }

    }

}
