using InfinityNetServer.Services.Tag.Presentation.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace InfinityNetServer.Services.File.Presentation
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
