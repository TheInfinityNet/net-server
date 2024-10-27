using InfinityNetServer.Services.File.Presentation.Configurations;
using Microsoft.AspNetCore.Builder;

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
