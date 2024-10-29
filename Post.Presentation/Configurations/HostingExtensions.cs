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
using InfinityNetServer.Services.Post.Application;
using InfinityNetServer.Services.Post.Infrastructure.DependencyInjection;
using InfinityNetServer.Services.Post.Infrastructure.Data;
using InfinityNetServer.BuildingBlocks.Presentation.Services;
using InfinityNetServer.Services.Post.Presentation.Services;
using InfinityNetServer.Services.Post.Presentation.Exceptions;

namespace InfinityNetServer.Services.Post.Presentation.Configurations;

internal static class HostingExtensions
{

    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        builder.AddSettings();

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddDbContext();

        builder.Services.AddMessageBus(builder.Configuration);

        builder.Services.AddRepositories();

        builder.Services.AddMappers();

        builder.Services.AddLocalization(builder.Configuration);

        builder.Services.AddGrpc();

        builder.Services.AddGrpcClients(builder.Configuration);

        builder.Services.AddCommonService();

        builder.Services.AddServices();

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
        var identityLocalizer = serviceProvider.GetRequiredService<IStringLocalizer<PostSharedResource>>();

        builder.Services.AddJwtAuthentication(builder.Configuration, localizer);

        builder.Services.AddDefaultCors(builder.Configuration);

        builder.Services.AddMetrics(builder.Configuration);

        builder.Services.AddGlobalExceptionHandler();

        builder.Services.AddGrpcPreConfigured();

        builder.Services.AddTransient<HttpPostExceptionHandler>();

        builder.Services.AddValidationHanlder(builder.Configuration, identityLocalizer);

        builder.AddCommonSerilog();

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app, IConfiguration configuration)
    {
        if (app == null) throw new ArgumentNullException(nameof(app));

        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

        app.UseLocalization();

        //app.UseHealthChecks();

        app.UseGlobalExceptionHandler();

        app.UseMiddleware<HttpPostExceptionHandler>();

        app.UseSwaggerPreConfigured();

        app.UsePathBase(configuration["Server:BasePath"]);

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseDefaultCors();

        app.UseMetrics(configuration);

        app.UseAuthentication();

        app.UseAuthorization();

        //app.AutoMigration();

        app.Services.SeedEssentialData(200);

        app.MapGrpcServices();

        app.MapControllers();

        return app;
    }

}
