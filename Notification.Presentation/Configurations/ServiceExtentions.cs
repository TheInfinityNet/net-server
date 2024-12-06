using InfinityNetServer.Services.Notification.Application.IServices;
using InfinityNetServer.Services.Notification.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InfinityNetServer.Services.Notification.Presentation.Configurations
{
    public static class ServiceExtentions
    {

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<INotificationService, NotificationService>();
        }

    }

}
