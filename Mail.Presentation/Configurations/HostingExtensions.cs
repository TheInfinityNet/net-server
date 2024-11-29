using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Serilog;
using InfinityNetServer.BuildingBlocks.Infrastructure.Bus;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Microsoft.Extensions.Configuration;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Localization;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Metric;
using InfinityNetServer.Services.Mail.Application.Consumers;
using InfinityNetServer.BuildingBlocks.Infrastructure.RabbitMQ;
using InfinityNetServer.Services.Mail.Application.Usecases;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Web;

namespace InfinityNetServer.Services.Mail.Presentation.Configurations;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.AddSettings();

        builder.Services.AddLocalization(builder.Configuration);

        builder.Services.AddMessageBus(builder.Configuration, typeof(SendMailWithCodeEventConsumer));

        builder.Services.AddMediatR(typeof(SendMailWithCodeEventHandler));

        builder.AddCommonSerilog();

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
