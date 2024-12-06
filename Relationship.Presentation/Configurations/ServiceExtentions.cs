using InfinityNetServer.Services.Relationship.Application.IServices;
using InfinityNetServer.Services.Relationship.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InfinityNetServer.Services.Relationship.Presentation.Configurations
{
    public static class ServiceExtentions
    {

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IFriendshipService, FriendshipService>();
            services.AddScoped<IProfileFollowService, ProfileFollowService>();
            services.AddScoped<IProfileBlockService, ProfileBlockService>();
        }

    }

}
