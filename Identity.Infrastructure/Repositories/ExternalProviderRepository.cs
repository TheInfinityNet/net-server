using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Identity.Domain.Entities;
using InfinityNetServer.Services.Identity.Domain.Enums;
using InfinityNetServer.Services.Identity.Domain.Repositories;
using InfinityNetServer.Services.Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Infrastructure.Repositories
{
    public class ExternalProviderRepository(IdentityDbContext context) : SqlRepository<ExternalProvider, Guid>(context), IExternalProviderRepository
    {
        public async Task<ExternalProvider> GetByAccountIdAndExternalNameAsync(Guid accountId, ExternalProviderName externalName)
            => await context.GoogleProviders.FirstOrDefaultAsync(x => x.AccountId == accountId && x.ExternalName == externalName);

        public async Task<ExternalProvider> GetByAccountIdAsync(Guid accountId)
            => await context.GoogleProviders.FirstOrDefaultAsync(x => x.AccountId == accountId);

    }

}
