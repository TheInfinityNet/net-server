using InfinityNetServer.BuildingBlocks.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InfinityNetServer.BuildingBlocks.Presentation.Services.BaseRedis
{
    public static class BaseRedisExtentions
    {

        public static void AddBaseRedisService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IBaseRedisService<,>), typeof(BaseRedisService<,>));
        }

    }

}
