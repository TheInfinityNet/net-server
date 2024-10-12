using InfinityNetServer.BuildingBlocks.Presentation.Configuration.MVC;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Serilog;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;
using InfinityNetServer.Services.Identity.Infrastructure.Data;
using InfinityNetServer.Services.Identity.Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using InfinityNetServer.Services.Identity.Presentation.Exceptions;
using InfinityNetServer.Services.Identity.Application;
using InfinityNetServer.BuildingBlocks.Presentation.Services.AuthenticatedUser;
using InfinityNetServer.BuildingBlocks.Presentation.Services.BaseRedis;
using InfinityNetServer.Services.Identity.Presentation.Services;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.ValidationHandler;
using InfinityNetServer.BuildingBlocks.Application;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Swagger;
using InfinityNetServer.BuildingBlocks.Infrastructure.Bus;
using InfinityNetServer.BuildingBlocks.Infrastructure.Redis;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Localization;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Jwt;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Metric;
using System;
using Microsoft.Extensions.Hosting;
using InfinityNetServer.BuildingBlocks.Infrastructure.Exceptions;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.CORS;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.HealthCheck;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Grpc;

namespace InfinityNetServer.Services.Identity.Presentation.Configurations;

internal static class HostingExtensions
{

    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        builder.AddSettings();

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddAuthenticatedUserService();

        builder.Services.AddBaseRedisService(builder.Configuration);

        builder.Services.AddAuthService(builder.Configuration);

        builder.Services.AddDbContext();

        builder.Services.AddRepositories();

        builder.Services.AddMessageBus(builder.Configuration);

        builder.Services.AddRedisConnection(builder.Configuration);

        builder.Services.AddGrpc();

        builder.Services.AddLocalization(builder.Configuration);

        builder.Services.AddHealthChecks(builder.Configuration);

        builder.Services.AddCors(builder.Configuration);

        builder.Services.AddControllers();

        //builder.Services.AddGrpcPreConfigured();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerPreConfigured(options =>
        {
            builder.Configuration.GetSection("Swagger").Bind(options);
        });

        builder.Services.AddWebServerOptions();

        var serviceProvider = builder.Services.BuildServiceProvider();
        var localizer = serviceProvider.GetRequiredService<IStringLocalizer<SharedResource>>();
        var identityLocalizer = serviceProvider.GetRequiredService<IStringLocalizer<IdentitySharedResource>>();

        builder.Services.AddJwtAuthentication(builder.Configuration, localizer);

        builder.Services.AddCors();

        builder.Services.AddMetrics(builder.Configuration);

        //builder.Services.AddApplicationExceptionHandlers();

        builder.Services.AddGlobalExceptionHandler();

        builder.Services.AddTransient<HttpIdentityExceptionHandler>();

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

        app.UseMiddleware<HttpIdentityExceptionHandler>();

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
