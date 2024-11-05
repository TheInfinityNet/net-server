using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Configuration;

namespace InfinityNetServer.BuildingBlocks.Infrastructure.RabbitMQ
{
    public static class RabbitMqExtentions
    {

        public static IServiceCollection AddMediatR(this IServiceCollection services, Type handlerAssemblyMarkerType)
            => services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(handlerAssemblyMarkerType.Assembly));

        public static IServiceCollection AddMassTransitConsumers(
            this IServiceCollection services,
            IConfiguration configuration,
            Type consumerAssemblyMarkerType)
        {
            var rabbitMqOptions = configuration.GetSection("RabbitMQ").Get<RabbitMqOptions>();

            return services.AddMassTransit(cfg =>
            {
                cfg.AddConsumers(consumerAssemblyMarkerType);

                cfg.UsingRabbitMq((context, bus) =>
                {
                    bus.Host(rabbitMqOptions.Host, rabbitMqOptions.VirtualHost, h =>
                    {
                        h.Username(rabbitMqOptions.Username);
                        h.Password(rabbitMqOptions.Password);
                    });

                    bus.UseMessageRetry(retry => 
                        retry.Incremental(
                            retryLimit: 3,
                            initialInterval: TimeSpan.FromSeconds(5),
                            intervalIncrement: TimeSpan.FromSeconds(5)
                        ));

                    bus.ConfigureEndpoints(context);
                    //bus.ReceiveEndpoint(
                    //    rabbitMqOptions.ExchangeName, e =>
                    //    {
                    //        e.ConfigureConsumers(context);
                    //    }
                    //);
                });
            });
        }

        public static void AddMassTransitProducers(this IServiceCollection services, IConfiguration configuration)
        {
            RabbitMqOptions rabbitMqOptions = configuration.GetSection("RabbitMQ").Get<RabbitMqOptions>();

            services.AddMassTransit(cfg =>
            {
                cfg.UsingRabbitMq((context, bus) =>
                {
                    bus.Host(rabbitMqOptions.Host, rabbitMqOptions.VirtualHost, h =>
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
