using InfinityNetServer.BuildingBlocks.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace InfinityNetServer.BuildingBlocks.Presentation.Services.AuthenticatedUser;

public static class AuthenticatedUserExtensions
{
    public static IServiceCollection AddAuthenticatedUserService(this IServiceCollection services)
    {
        return services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
    }
}
