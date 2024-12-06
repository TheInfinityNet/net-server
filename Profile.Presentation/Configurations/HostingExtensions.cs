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
using InfinityNetServer.Services.Profile.Application;
using InfinityNetServer.Services.Profile.Application.Consumers;
using InfinityNetServer.Services.Profile.Application.Usecases;
using InfinityNetServer.Services.Profile.Infrastructure.Data;
using InfinityNetServer.Services.Profile.Infrastructure.DependencyInjection;
using InfinityNetServer.Services.Profile.Presentation.Exceptions;
using InfinityNetServer.Services.Profile.Presentation.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Serilog;
using System;

namespace InfinityNetServer.Services.Profile.Presentation.Configurations;

internal static class HostingExtensions
{

    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.AddSettings();

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddDbContext();

        builder.Services.AddMessageBus(builder.Configuration, 
            typeof(CreateUserProfileConsumer), 
            typeof(ActiveProfileConsumer));

        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CreateUserProfileHandler).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(ActiveProfileHandler).Assembly);
        });

        builder.Services.AddRepositories();

        builder.Services.AddMappers(typeof(ProfileMapper));

        builder.Services.AddLocalization(builder.Configuration);

        builder.Services.AddGrpc();

        builder.Services.AddGrpcClients(builder.Configuration);

        builder.Services.AddCommonService();

        builder.Services.AddServices();

        //builder.Services.AddHealthChecks(builder.Configuration);

        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
        });

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

        builder.Services.AddDefaultCors(builder.Configuration);

        builder.Services.AddMetrics(builder.Configuration);

        builder.Services.AddGlobalExceptionHandler();

        builder.Services.AddGrpcPreConfigured();

        builder.Services.AddTransient<HttpProfileExceptionHandler>();

        builder.Services.AddValidationHanlder(builder.Configuration, identityLocalizer);

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

        app.UseMiddleware<HttpProfileExceptionHandler>();

        app.UseSwaggerPreConfigured();

        app.UsePathBase(configuration["Server:BasePath"]);

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseDefaultCors();

        app.UseMetrics(configuration);

        app.UseAuthentication();

        app.UseAuthorization();

        app.AutoMigration();

        app.Services.SeedEssentialData(100);

        app.MapGrpcServices();

        app.MapControllers();

        return app;
    }

}
