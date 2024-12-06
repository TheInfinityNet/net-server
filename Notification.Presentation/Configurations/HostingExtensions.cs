using InfinityNetServer.BuildingBlocks.Application;
using InfinityNetServer.BuildingBlocks.Infrastructure.Bus;
using InfinityNetServer.BuildingBlocks.Infrastructure.RabbitMQ;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.CORS;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Grpc;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Jwt;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Localization;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Metric;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Serilog;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Swagger;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.ValidationHandler;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Web;
using InfinityNetServer.BuildingBlocks.Presentation.Mappers;
using InfinityNetServer.Services.Notification.Application;
using InfinityNetServer.Services.Notification.Application.Consumers;
using InfinityNetServer.Services.Notification.Application.Usecases;
using InfinityNetServer.Services.Notification.Infrastructure.DependencyInjection;
using InfinityNetServer.Services.Notification.Presentation.Exceptions;
using InfinityNetServer.Services.Notification.Presentation.Mappers;
using InfinityNetServer.Services.Notification.Presentation.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Serilog;
using System;

namespace InfinityNetServer.Services.Notification.Presentation.Configurations;

internal static class HostingExtensions
{

    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.AddSettings();

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddDbContext(builder.Configuration);

        builder.Services.AddMessageBus(builder.Configuration,
            typeof(CreatePostNotificationConsumer),
            typeof(CreateCommentNotificationConsumer),
            typeof(CreateFriendshipNotificationConsumer),
            typeof(CreateProfileFollowNotificationConsumer),
            typeof(CreatePostReactionNotificationConsumer),
            typeof(CreateCommentReactionNotificationConsumer));

        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CreateCommentNotificationHandler).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(CreatePostNotificationHandler).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(CreateFriendshipNotificationHandler).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(CreateProfileFollowNotificationHandler).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(CreatePostReactionNotificationHandler).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(CreateCommentReactionNotificationHandler).Assembly);
        });

        builder.Services.AddRepositories();

        builder.Services.AddMappers(typeof(NotificationMapper));

        builder.Services.AddGrpc();

        builder.Services.AddGrpcClients(builder.Configuration);

        builder.Services.AddCommonService();

        builder.Services.AddServices();

        builder.Services.AddLocalization(builder.Configuration);

        //builder.Services.AddHealthChecks(builder.Configuration);

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerPreConfigured(options =>
        {
            builder.Configuration.GetSection("Swagger").Bind(options);
        });

        builder.Services.AddWebServerOptions();

        var serviceProvider = builder.Services.BuildServiceProvider();
        var localizer = serviceProvider.GetRequiredService<IStringLocalizer<SharedResource>>();
        var notificationLocalizer = serviceProvider.GetRequiredService<IStringLocalizer<NotificationSharedResource>>();

        builder.Services.AddJwtAuthentication(builder.Configuration, localizer);

        builder.Services.AddDefaultCors(builder.Configuration);

        builder.Services.AddMetrics(builder.Configuration);

        builder.Services.AddGlobalExceptionHandler();

        builder.Services.AddGrpcPreConfigured();

        builder.Services.AddTransient<HttpNotificationExceptionHandler>();

        builder.Services.AddValidationHanlder(builder.Configuration, notificationLocalizer);

        builder.AddCommonSerilog();

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

        app.UseLocalization();

        //app.UseHealthChecks();

        app.UseGlobalExceptionHandler();

        app.UseMiddleware<HttpNotificationExceptionHandler>();

        app.UseSwaggerPreConfigured();

        app.UsePathBase(configuration["Server:BasePath"]);

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseDefaultCors();

        app.UseMetrics(configuration);

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapGrpcServices();

        app.MapControllers();

        return app;
    }

}
