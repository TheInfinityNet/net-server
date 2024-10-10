using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using InfinityNetServer.Services.Identity.Infrastructure.Data;
using InfinityNetServer.Services.Identity.Infrastructure.Repositories;

namespace InfinityNetServer.Services.Identity.Infrastructure.DependencyInjection;

public static class InfrastructureModule
{

    public static void AddDbContext(this IServiceCollection services)
    {
        services.AddDbContext<IdentityDbContext>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(AccountRepository));
        services.AddScoped(typeof(AccountProviderRepository));
        services.AddScoped(typeof(VerificationRepository));
    }

}
