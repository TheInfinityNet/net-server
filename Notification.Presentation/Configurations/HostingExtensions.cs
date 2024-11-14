using InfinityNetServer.BuildingBlocks.Presentation.Configuration.MVC;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Serilog;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.ValidationHandler;
using InfinityNetServer.BuildingBlocks.Application;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Swagger;
using InfinityNetServer.BuildingBlocks.Infrastructure.Bus;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Localization;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Jwt;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Metric;
using System;
using Microsoft.Extensions.Hosting;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.CORS;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.HealthCheck;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Grpc;
using InfinityNetServer.BuildingBlocks.Presentation.Mappers;
using InfinityNetServer.BuildingBlocks.Presentation.Services;
using Elastic.CommonSchema;
using InfinityNetServer.BuildingBlocks.Infrastructure.Redis;
using InfinityNetServer.BuildingBlocks.Infrastructure.RabbitMQ;
using InfinityNetServer.Services.Notification.Infrastructure.DependencyInjection;
using InfinityNetServer.Services.Notification.Application.Consumers;
using InfinityNetServer.Services.Notification.Application;
using InfinityNetServer.Services.Notification.Application.Usecases;
using InfinityNetServer.Services.Notification.Presentation.Exceptions;
using InfinityNetServer.Services.Notification.Presentation.Services;

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
            typeof(CreatePostNotificationCommandConsumer), 
            typeof(CreateCommentNotificationCommandConsumer),
            typeof(CreateFriendshipNotificationCommandConsumer),
            typeof(CreateProfileFollowNotificationCommandConsumer));

        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CreateCommentNotificationCommandHandler).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(CreatePostNotificationCommandHandler).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(CreateFriendshipNotificationCommandHandler).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(CreateProfileFollowNotificationCommandHandler).Assembly);
        });

        builder.Services.AddRepositories();

        builder.Services.AddMappers();

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
