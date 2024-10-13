using InfinityNetServer.Services.Profile.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InfinityNetServer.Services.Profile.Presentation.Services
{
    public static class ServiceExtentions
    {

        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserProfileService, UserProfileService>();
        }

    }

}
