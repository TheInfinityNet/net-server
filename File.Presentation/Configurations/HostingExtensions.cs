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
using InfinityNetServer.Services.File.Infrastructure.Data;
using InfinityNetServer.BuildingBlocks.Presentation.Services;
using InfinityNetServer.Services.File.Infrastructure.DependencyInjection;
using InfinityNetServer.Services.File.Application;
using InfinityNetServer.Services.File.Presentation.Services;
using InfinityNetServer.Services.File.Presentation.Exceptions;
using Elastic.CommonSchema;
using InfinityNetServer.BuildingBlocks.Infrastructure.Redis;
using InfinityNetServer.Services.File.Infrastructure.Minio;
using InfinityNetServer.BuildingBlocks.Infrastructure.RabbitMQ;
using InfinityNetServer.Services.File.Application.Consumers;
using InfinityNetServer.Services.File.Application.Usecases;

namespace InfinityNetServer.Services.File.Presentation.Configurations;

internal static class HostingExtensions
{

    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.AddSettings();

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddDbContext(builder.Configuration);

        builder.Services.AddMessageBus(builder.Configuration, typeof(CreatePhotoMetadataEventConsumer));

        builder.Services.AddMediatR(typeof(CreatePhotoMetadataEventHandler));

        builder.Services.AddRedisConnection(builder.Configuration);

        builder.Services.AddMinioClient(builder.Configuration);

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
        var fileLocalizer = serviceProvider.GetRequiredService<IStringLocalizer<FileSharedResource>>();

        builder.Services.AddJwtAuthentication(builder.Configuration, localizer);

        builder.Services.AddDefaultCors(builder.Configuration);

        builder.Services.AddMetrics(builder.Configuration);

        builder.Services.AddGlobalExceptionHandler();

        builder.Services.AddGrpcPreConfigured();

        builder.Services.AddTransient<HttpFileExceptionHandler>();

        builder.Services.AddValidationHanlder(builder.Configuration, fileLocalizer);

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

        app.UseMiddleware<HttpFileExceptionHandler>();

        app.UseSwaggerPreConfigured();

        app.UsePathBase(configuration["Server:BasePath"]);

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseDefaultCors();

        app.UseMetrics(configuration);

        app.UseAuthentication();

        app.UseAuthorization();

        app.Services.SeedEssentialData();

        app.MapGrpcServices();

        app.MapControllers();

        return app;
    }

}
