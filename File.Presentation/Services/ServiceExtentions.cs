using InfinityNetServer.Services.File.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InfinityNetServer.Services.File.Presentation.Services
{
    public static class ServiceExtentions
    {

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IMinioClientService, MinioClientService>();
        }

    }

}
