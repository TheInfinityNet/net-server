using InfinityNetServer.BuildingBlocks.Infrastructure.Bus;
using InfinityNetServer.BuildingBlocks.Infrastructure.RabbitMQ;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Localization;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Metric;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Serilog;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Web;
using InfinityNetServer.Services.Mail.Application.Consumers;
using InfinityNetServer.Services.Mail.Application.Usecases;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace InfinityNetServer.Services.Mail.Presentation.Configurations;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.AddSettings();

        builder.Services.AddLocalization(builder.Configuration);

        builder.Services.AddServices();

        builder.Services.AddMessageBus(builder.Configuration, typeof(SendMailWithCodeConsumer));

        builder.Services.AddMediatR(typeof(SendMailWithCodeHandler));

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
