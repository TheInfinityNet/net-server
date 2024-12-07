using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Identity.Domain.Entities;
using InfinityNetServer.Services.Identity.Domain.Repositories;
using InfinityNetServer.Services.Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Infrastructure.Repositories
{
    public class GoogleProviderRepository(IdentityDbContext context) : SqlRepository<GoogleProvider, Guid>(context), IGoogleProviderRepository
    {

        public async Task<GoogleProvider> GetByAccountIdAsync(Guid accountId)
            => await context.GoogleProviders.FirstOrDefaultAsync(x => x.AccountId == accountId);

    }

}
