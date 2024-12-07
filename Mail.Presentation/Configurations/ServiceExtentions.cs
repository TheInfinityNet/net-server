using InfinityNetServer.Services.Mail.Application.IServices;
using InfinityNetServer.Services.Mail.Infrastructure.SmtpMail;
using Microsoft.Extensions.DependencyInjection;

namespace InfinityNetServer.Services.Mail.Presentation.Configurations
{
    public static class ServiceExtentions
    {

        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IMailService, MailService>();
        }

    }

}
