using Microsoft.Extensions.DependencyInjection;
using System;

namespace InfinityNetServer.BuildingBlocks.Presentation.Mappers;

public static class MapperExtensions
{

    public static void AddMapper(this IServiceCollection services, Type mappingProfileAssemblyMarkerType = null)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        if (mappingProfileAssemblyMarkerType != null) services.AddAutoMapper(mappingProfileAssemblyMarkerType);
        services.AddAutoMapper(typeof(CommonMapper));
    }
}
