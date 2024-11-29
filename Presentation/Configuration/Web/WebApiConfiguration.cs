using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using InfinityNetServer.BuildingBlocks.Presentation.Exceptions;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace InfinityNetServer.BuildingBlocks.Presentation.Configuration.Web;

public static class WebApiConfiguration
{

    public static void AddSettings(this IHostApplicationBuilder builder)
    {
        builder.Configuration
            .SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        if (builder.Environment.IsDevelopment()) builder.Configuration.AddJsonFile("appsettings.Local.json", true, true);
    }

    public static void AddWebServerOptions(this IServiceCollection services)
    {
        // If using Kestrel:
        services.Configure<KestrelServerOptions>(options =>
        {
            options.AllowSynchronousIO = true;
        });
        // If using IIS:
        services.Configure<IISServerOptions>(options =>
        {
            options.AllowSynchronousIO = true;
        });
    }

    public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<HttpGlobalExceptionHandler>();
    }

    public static void AddGlobalExceptionHandler(this IServiceCollection services)
    {
        services.AddTransient<HttpGlobalExceptionHandler>();
    }

}
