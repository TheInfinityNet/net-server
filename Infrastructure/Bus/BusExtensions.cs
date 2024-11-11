using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Infrastructure.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace InfinityNetServer.BuildingBlocks.Infrastructure.Bus;

public static class BusExtensions
{

    public static void AddMessageBus(this IServiceCollection services, IConfiguration configuration, params Type[] consumerAssemblyMarkerType)
    {
        if (consumerAssemblyMarkerType.Length != 0) services.AddMassTransitConsumers(configuration, consumerAssemblyMarkerType);
        else services.AddMassTransitProducers(configuration);
        services.AddScoped<IMessageBus, MessageBus>();
    }

}
