using InfinityNetServer.Services.Identity.Presentation.Configurations;
using Microsoft.AspNetCore.Builder;

namespace InfinityNetServer.Services.Identity.Presentation;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var app = builder.ConfigureServices().ConfigurePipeline(builder.Configuration);

        app.Run();
    }

}
