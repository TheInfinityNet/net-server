﻿using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Identity.Domain.Repositories;
using InfinityNetServer.Services.Identity.Infrastructure.Data;
using InfinityNetServer.Services.Identity.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace InfinityNetServer.Services.Identity.Infrastructure.DependencyInjection;

public static class InfrastructureModule
{

    public static void AddDbContext(this IServiceCollection services)
    {
        services.AddDbContext<IdentityDbContext>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(ISqlRepository<,>), typeof(SqlRepository<,>));
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ILocalProviderRepository, LocalProviderRepository>();
        services.AddScoped<IExternalProviderRepository, ExternalProviderRepository>();
        services.AddScoped<IVerificationRepository, VerificationRepository>();
    }

}
