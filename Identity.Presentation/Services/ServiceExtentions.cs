using InfinityNetServer.Services.Identity.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InfinityNetServer.Services.Identity.Presentation.Services
{
    public static class ServiceExtentions
    {

        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAccountService, AccountService>();
        }

    }

}
