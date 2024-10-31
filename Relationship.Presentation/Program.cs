using InfinityNetServer.Services.Relationship.Presentation.Configurations;
using Microsoft.AspNetCore.Builder;

namespace InfinityNetServer.Services.Relationship.Presentation
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
