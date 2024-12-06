using InfinityNetServer.Services.Identity.Application.IServices;
using InfinityNetServer.Services.Identity.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InfinityNetServer.Services.Identity.Presentation.Configurations
{
    public static class ServiceExtentions
    {

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ILocalProviderService, LocalProviderService>();
            services.AddScoped<IVerificationService, VerificationService>();
        }

    }

}
