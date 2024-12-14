using Microsoft.Extensions.DependencyInjection;
using System;

namespace InfinityNetServer.BuildingBlocks.Presentation.Mappers;

public static class MapperExtensions
{

    public static void AddMappers(this IServiceCollection services, params Type[] mappingProfileAssemblyMarkerType)
    {
        if (mappingProfileAssemblyMarkerType != null) services.AddAutoMapper(mappingProfileAssemblyMarkerType);
        services.AddAutoMapper(typeof(CommonMappers));
    }
}
