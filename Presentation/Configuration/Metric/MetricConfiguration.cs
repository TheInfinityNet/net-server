using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prometheus;

namespace InfinityNetServer.BuildingBlocks.Presentation.Configuration.Metric;

public static class ValidationHandlerConfiguration
{
    public static void AddMetrics(this IServiceCollection services, IConfiguration configuration)
    {
        var metricOptions = configuration.GetSection("Metric").Get<MetricOptions>();

        if (metricOptions.StandAloneKestrelServerEnabled)
        {
            services.AddMetricServer(options =>
            {
                options.Port = metricOptions.Port;
                options.Url = metricOptions.Url;
                options.Hostname = metricOptions.Hostname;
            });
            services.AddOpenTelemetry();
        }

        services.AddSingleton<IMetricFactory>(Metrics.DefaultFactory);
    }

    public static void UseMetrics(this IApplicationBuilder app, IConfiguration configuration)
    {
        var metricOptions = configuration.GetSection("Metric").Get<MetricOptions>();

        if (!metricOptions.StandAloneKestrelServerEnabled)
            app.UseMetricServer(metricOptions.Port, metricOptions.Url);

        if (metricOptions.HttpMetricsEnabled)
        {
            app.UseHttpMetrics(options =>
            {
                options.AddCustomLabel("host", context => context.Request.Host.Host);
            });
        }

        if (metricOptions.SuppressDefaultMetrics) Metrics.SuppressDefaultMetrics();
    }
}
