using InfinityNetServer.BuildingBlocks.Presentation.Configuration.MVC;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Serilog;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using InfinityNetServer.BuildingBlocks.Presentation.Services.AuthenticatedUser;
using InfinityNetServer.BuildingBlocks.Presentation.Services.BaseRedis;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.ValidationHandler;
using InfinityNetServer.BuildingBlocks.Application;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Swagger;
using InfinityNetServer.BuildingBlocks.Infrastructure.Bus;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Localization;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Jwt;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Metric;
using System;
using Microsoft.Extensions.Hosting;
using InfinityNetServer.Services.Profile.Presentation.Exceptions;
using InfinityNetServer.Services.Profile.Application;
using InfinityNetServer.Services.Profile.Presentation.Services;
using InfinityNetServer.Services.Profile.Infrastructure.DependencyInjection;
using InfinityNetServer.Services.Profile.Infrastructure.Data;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.CORS;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.HealthCheck;
using InfinityNetServer.Services.Profile.Application.Consumers;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Grpc;

namespace InfinityNetServer.Services.Profile.Presentation.Configurations;

internal static class HostingExtensions
{

    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        builder.AddSettings();

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddAuthenticatedUserService();

        builder.Services.AddBaseRedisService(builder.Configuration);

        builder.Services.AddProfileService(builder.Configuration);

        builder.Services.AddDbContext();

        builder.Services.AddRepositories();

        //builder.Services.AddGrpc();

        builder.Services.AddLocalization(builder.Configuration);

        builder.Services.AddMessageBus(builder.Configuration, typeof(ProfileCreatedConsumer));

        builder.Services.AddHealthChecks(builder.Configuration);

        builder.Services.AddCors(builder.Configuration);

        builder.Services.AddControllers();

        builder.Services.AddGrpcPreConfigured();

        builder.Services.AddGrpcClients(builder.Configuration);

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerPreConfigured(options =>
        {
            builder.Configuration.GetSection("Swagger").Bind(options);
        });

        builder.Services.AddWebServerOptions();

        var serviceProvider = builder.Services.BuildServiceProvider();
        var localizer = serviceProvider.GetRequiredService<IStringLocalizer<SharedResource>>();
        var identityLocalizer = serviceProvider.GetRequiredService<IStringLocalizer<ProfileSharedResource>>();

        builder.Services.AddJwtAuthentication(builder.Configuration, localizer);

        builder.Services.AddCors();

        builder.Services.AddMetrics(builder.Configuration);

        builder.Services.AddGlobalExceptionHandler();

        builder.Services.AddTransient<HttpProfileExceptionHandler>();

        builder.Services.AddValidationHanlder(builder.Configuration, identityLocalizer);

        builder.AddCustomSerilog();

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app, IConfiguration configuration)
    {
        if (app == null) throw new ArgumentNullException(nameof(app));

        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

        app.UseLocalization();

        app.UseHealthChecks();

        app.UseGlobalExceptionHandler();

        app.UseMiddleware<HttpProfileExceptionHandler>();

        app.UseSwaggerPreConfigured();

        app.UsePathBase(configuration["Server:BasePath"]);

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseCommonCors();

        app.UseMetrics(configuration);

        app.UseAuthentication();

        app.UseAuthorization();

        app.Services.SeedEssentialData(10);

        app.MapGrpcServices();

        app.MapControllers();

        return app;
    }

}