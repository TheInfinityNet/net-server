using InfinityNetServer.BuildingBlocks.Presentation.Configuration.MVC;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Serilog;
using InfinityNetServer.BuildingBlocks.Infrastructure.Bus;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Microsoft.Extensions.Configuration;
using InfinityNetServer.Services.Mail.Application.Consumers;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Localization;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Metric;

namespace InfinityNetServer.Services.Mail.Presentation.Configurations;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.AddSettings();

        builder.Services.AddLocalization(builder.Configuration);

        builder.Services.AddMessageBus(builder.Configuration, typeof(MailSentConsumer));

        builder.AddCustomSerilog();

        builder.Services.AddMetrics(builder.Configuration);

        builder.Services.AddGlobalExceptionHandler();

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app, IConfiguration configuration)
    {
        app.UseSerilogRequestLogging();

        app.UseLocalization();

        app.UseGlobalExceptionHandler();

        app.UseMetrics(configuration);

        return app;
    }


}
