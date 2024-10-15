using InfinityNetServer.BuildingBlocks.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InfinityNetServer.BuildingBlocks.Presentation.Services;

public static class ServiceExtensions
{
    public static void AddCommonService(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
        services.AddScoped(typeof(IBaseRedisService<,>), typeof(BaseRedisService<,>));
    }
}
