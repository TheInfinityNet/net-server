using InfinityNetServer.Services.Reaction.Application.IServices;
using InfinityNetServer.Services.Reaction.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InfinityNetServer.Services.Reaction.Presentation.Configurations
{
    public static class ServiceExtentions
    {

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IPostReactionService, PostReactionService>();
            services.AddScoped<ICommentReactionService, CommentReactionService>();
        }

    }

}
