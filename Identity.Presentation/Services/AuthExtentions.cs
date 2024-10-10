using InfinityNetServer.Services.Identity.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InfinityNetServer.Services.Identity.Presentation.Services
{
    public static class AuthExtentions
    {

        public static void AddAuthService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthService, AuthService>();
        }

    }

}
