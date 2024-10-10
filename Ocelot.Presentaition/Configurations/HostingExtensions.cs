using InfinityNetServer.BuildingBlocks.Presentation.Configuration.MVC;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Serilog;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Grpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;
using Ocelot.Provider.Polly;
using Ocelot.Cache.CacheManager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using InfinityNetServer.BuildingBlocks.Application;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Jwt;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Localization;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Metric;

namespace InfinityNetServer.Gateways.OcelotGateway.Presentaition.Configurations;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.AddSettings();

        builder.Services.AddLocalization(builder.Configuration);

        builder.Services.AddControllers();

        builder.Configuration.AddOcelotWithSwaggerSupport(
            (o) =>
            {
                o.Folder = "Configurations/Settings";
            }
        );

        builder.Services.AddGrpcPreConfigured();

        builder.Services.AddGrpcClients(builder.Configuration);

        var serviceProvider = builder.Services.BuildServiceProvider();
        var localizer = serviceProvider.GetRequiredService<IStringLocalizer<SharedResource>>();

        builder.Services.AddJwtAuthentication(builder.Configuration, localizer, true);

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddOcelot()
            .AddCacheManager(options =>
            {
                options.WithDictionaryHandle();
            })
            .AddPolly();

        builder.Services.AddSwaggerForOcelot(builder.Configuration);

        builder.Services.AddCors();

        builder.AddCustomSerilog();

        builder.Services.AddMetrics(builder.Configuration);

        builder.Services.AddGlobalExceptionHandler();

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app, IConfiguration configuration)
    {
        app.UseSerilogRequestLogging();

        app.UseStaticFiles();

        app.UseLocalization();

        app.UseGlobalExceptionHandler();

        app.UseSwaggerForOcelotUI(opt =>
        {
            opt.PathToSwaggerGenerator = "/swagger/docs";
        });

        app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

        app.UseMetrics(configuration);

        app.UseOcelot().Wait();

        return app;
    }

}
