using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace InfinityNetServer.BuildingBlocks.Infrastructure.Exceptions;

public static class ExceptionExtensions
{

    public static void AddApplicationExceptionHandlers(this IServiceCollection services)
    {
        services.AddManagedExceptionsHandler();
    }

    private static void AddManagedExceptionsHandler(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRequestExceptionHandler<,,>), typeof(ManagedExceptionHandler<,,,>));
    }

}
