using Microsoft.Extensions.DependencyInjection;
using InfinityNetServer.Services.Profile.Infrastructure.Data;
using InfinityNetServer.Services.Profile.Infrastructure.Repositories;
using InfinityNetServer.Services.Profile.Domain.Repositories;

namespace InfinityNetServer.Services.Profile.Infrastructure.DependencyInjection;

public static class InfrastructureModule
{

    public static void AddDbContext(this IServiceCollection services)
    {
        services.AddDbContext<ProfileDbContext>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IPageProfileRepository, PageProfileRepository>();
        services.AddScoped<IUserProfileRepository, UserProfileRepository>();
    }

}
