using InfinityNetServer.Services.Profile.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InfinityNetServer.Services.Profile.Presentation.Services
{
    public static class ServiceExtentions
    {

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IUserProfileService, UserProfileService>();
        }

    }

}
