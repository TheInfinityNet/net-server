using InfinityNetServer.Services.Comment.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InfinityNetServer.Services.Comment.Presentation.Services
{
    public static class ServiceExtentions
    {

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICommentService, CommentService>();
        }

    }

}
