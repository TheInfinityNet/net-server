using Microsoft.Extensions.DependencyInjection;
using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Group.Infrastructure.Data;

namespace InfinityNetServer.Services.Group.Infrastructure.DependencyInjection;

public static class InfrastructureModule
{

    public static void AddDbContext(this IServiceCollection services)
    {
        services.AddDbContext<GroupDbContext>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(ISqlRepository<,>), typeof(SqlRepository<,>));
        //services.AddScoped<IPageProfileRepository, PageProfileRepository>();
    }

}
