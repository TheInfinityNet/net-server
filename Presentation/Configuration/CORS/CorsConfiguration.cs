using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InfinityNetServer.BuildingBlocks.Presentation.Configuration.CORS
{
    public static class HealthCheckConfiguration
    {

        private static string DEFAULT_CORS_POLICY = "DefaultCorsPolicy";

        public static void AddCors(this IServiceCollection services, IConfiguration configuration)
        {
            var corsOptions = configuration.GetSection("Cors").Get<CorsOptions>();
            services.AddCors(options =>
            {
                options.AddPolicy(DEFAULT_CORS_POLICY,
                    policy =>
                    {
                        policy.WithOrigins(corsOptions.AllowOrigins) // Cho phép các domain cụ thể
                              .AllowAnyHeader()  // Cho phép bất kỳ header nào
                              .AllowAnyMethod(); // Cho phép tất cả các phương thức HTTP (GET, POST, PUT, DELETE, v.v.)
                    });
            });
        }

        public static void UseCommonCors(this IApplicationBuilder app)
        {
            app.UseCors(DEFAULT_CORS_POLICY);
        }

    }

}
