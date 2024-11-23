using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using InfinityNetServer.Services.Reaction.Application.Services;

namespace InfinityNetServer.Services.Reaction.Presentation.Services
{
    public static class ServiceExtentions
    {

        public static void AddServices(this IServiceCollection services)
        {
            //services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddScoped<IPostReactionService, PostReactionService>();
            services.AddScoped<ICommentReactionService, CommentReactionService>();
        }

    }

}
