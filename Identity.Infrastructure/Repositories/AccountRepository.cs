using InfinityNetServer.Services.Identity.Domain.Entities;
using InfinityNetServer.Services.Identity.Infrastructure.Data;
using System;
using InfinityNetServer.Services.Identity.Domain.Repositories;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Infrastructure.Repositories
{
    public class AccountRepository(IdentityDbContext context) : SqlRepository<Account, Guid>(context), IAccountRepository
    {

        public async Task<Account> GetByDefaultUserProfileIdAsync(Guid defaultUserProfileId)
            => await context.Accounts.FirstOrDefaultAsync(x => x.DefaultUserProfileId == defaultUserProfileId);

    }
}
