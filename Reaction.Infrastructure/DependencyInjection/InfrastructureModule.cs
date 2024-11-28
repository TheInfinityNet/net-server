using Microsoft.Extensions.DependencyInjection;
using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Reaction.Infrastructure.Data;
using InfinityNetServer.Services.Reaction.Domain.Repositories;
using InfinityNetServer.Services.Reaction.Infrastructure.Repositories;

namespace InfinityNetServer.Services.Reaction.Infrastructure.DependencyInjection;

public static class InfrastructureModule
{

    public static void AddDbContext(this IServiceCollection services)
    {
        services.AddDbContext<ReactionDbContext>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(ISqlRepository<,>), typeof(SqlRepository<,>));
        services.AddScoped<IPostReactionRepository, PostReactionRepository>();
        services.AddScoped<ICommentReactionRepository, CommentReactionRepository>();

    }

}
