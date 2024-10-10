using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System;
using System.Reflection;
using InfinityNetServer.BuildingBlocks.Application.Attributes;
using Microsoft.Extensions.Configuration;

namespace InfinityNetServer.BuildingBlocks.Infrastructure.RabbitMQ
{
    public static class RabbitMqExtentions
    {

        public static void AddMassTransitConsumers(
            this IServiceCollection services,
            IConfiguration configuration,
            Type consumerAssemblyMarkerType)
        {
            var rabbitMqOptions = configuration.GetSection("RabbitMQ").Get<RabbitMqOptions>();

            services.AddMassTransit(x =>
            {
                x.AddConsumers(consumerAssemblyMarkerType.Assembly);

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqOptions.Host, rabbitMqOptions.VirtualHost, h =>
                    {
                        h.Username(rabbitMqOptions.Username);
                        h.Password(rabbitMqOptions.Password);
                    });

                    // Lấy tất cả các consumer đã đăng ký
                    var consumerTypes = consumerAssemblyMarkerType.Assembly.GetTypes()
                        .Where(t => t.IsClass && !t.IsAbstract && typeof(IConsumer).IsAssignableFrom(t))
                        .ToList();

                    foreach (var consumerType in consumerTypes)
                    {
                        // Kiểm tra xem consumer có QueueNameAttribute không
                        var queueNameAttribute = consumerType.GetCustomAttribute<QueueNameAttribute>();
                        // Tạo tên queue dựa trên ExchangeName và tên consumer
                        var queueName = queueNameAttribute != null
                                    ? $"{rabbitMqOptions.ExchangeName}-{queueNameAttribute.QueueName}-queue"
                                    : $"{rabbitMqOptions.ExchangeName}-{GetQueueName(consumerType)}-queue";

                        cfg.ReceiveEndpoint(queueName, e =>
                        {
                            // Cấu hình consumer cho endpoint này
                            e.ConfigureConsumer(context, consumerType);

                            // Tùy chọn cấu hình thêm cho endpoint nếu cần
                            e.PrefetchCount = 16;
                            e.ConcurrentMessageLimit = 8;
                            e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                        });
                    }

                    // Tùy chọn cấu hình thêm nếu cần
                    cfg.ConfigureEndpoints(context);
                });
            });
        }

        public static void AddMassTransitProducers(this IServiceCollection services, IConfiguration configuration)
        {
            RabbitMqOptions rabbitMqOptions = configuration.GetSection("RabbitMQ").Get<RabbitMqOptions>();

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqOptions.Host, rabbitMqOptions.VirtualHost, h =>
                    {
                        h.Username(rabbitMqOptions.Username);
                        h.Password(rabbitMqOptions.Password);
                    });
                });
            });
        }

        // Hàm hỗ trợ để lấy tên queue từ tên consumer
        private static string GetQueueName(Type consumerType)
        {
            // Ví dụ: OrderCreatedConsumer => order-created
            var name = consumerType.Name;
            if (name.EndsWith("Consumer"))
            {
                name = name[..^"Consumer".Length];
            }
            return name.Replace(" ", "-").ToLower();
        }

    }

}
