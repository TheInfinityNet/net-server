using InfinityNetServer.BuildingBlocks.Application.IServices;
using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Infrastructure.Redis;
using Microsoft.Extensions.DependencyInjection;

namespace InfinityNetServer.BuildingBlocks.Presentation.Configuration.Web;

public static class ServiceExtensions
{
    public static void AddCommonService(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
        services.AddScoped(typeof(IBaseRedisService<,>), typeof(BaseRedisService<,>));
    }
}
