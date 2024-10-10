using InfinityNetServer.Gateways.OcelotGateway.Presentaition.Configurations;
using Microsoft.AspNetCore.Builder;

namespace InfinityNetServer.Gateways.OcelotGateway;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var app = builder.ConfigureServices().ConfigurePipeline(builder.Configuration);

        app.Run();
    }
}