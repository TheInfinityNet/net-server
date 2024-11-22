using InfinityNetServer.Services.Notification.Presentation.Configurations;
using Microsoft.AspNetCore.Builder;

namespace InfinityNetServer.Services.Notification.Presentation
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var app = builder.ConfigureServices().ConfigurePipeline(builder.Configuration);

            app.Run();
        }

    }
}
