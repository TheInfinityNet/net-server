using InfinityNetServer.Services.Post.Application.IServices;
using InfinityNetServer.Services.Post.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InfinityNetServer.Services.Post.Presentation.Configurations
{
    public static class ServiceExtentions
    {

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IUserTimelineService, UserTimelineService>();
        }

    }

}
