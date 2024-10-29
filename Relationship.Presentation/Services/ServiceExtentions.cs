using InfinityNetServer.Services.Relationship.Application;
using Microsoft.Extensions.DependencyInjection;

namespace InfinityNetServer.Services.Relationship.Presentation.Services
{
    public static class ServiceExtentions
    {

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IFriendshipService, FriendshipService>();
        }

    }

}
