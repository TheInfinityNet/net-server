using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Identity.Domain.Entities;
using InfinityNetServer.Services.Identity.Domain.Repositories;
using InfinityNetServer.Services.Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Infrastructure.Repositories
{
    public class FacebookProviderRepository(IdentityDbContext context) : SqlRepository<FacebookProvider, Guid>(context), IFacebookProviderRepository
    {

        public async Task<FacebookProvider> GetByAccountIdAsync(Guid accountId)
            => await context.FacebookProviders.FirstOrDefaultAsync(x => x.AccountId == accountId);

    }

}
