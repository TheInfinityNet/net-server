using InfinityNetServer.Services.Profile.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InfinityNetServer.Services.Profile.Presentation.Services
{
    public static class ProfileExtentions
    {

        public static void AddProfileService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IProfileService, ProfileService>();
        }

    }

}
