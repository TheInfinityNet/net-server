using InfinityNetServer.Services.Identity.Domain.Entities;
using InfinityNetServer.Services.Identity.Infrastructure.Data;
using System;
using InfinityNetServer.Services.Identity.Domain.Repositories;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Infrastructure.Repositories
{
    public class AccountRepository : SqlRepository<Account, Guid>, IAccountRepository
    {

        public AccountRepository(IdentityDbContext context) : base(context)
        { }

        public async Task<Account> GetByDefaultUserProfileIdAsync(Guid defaultUserProfileId)
        {
            return await ((IdentityDbContext)_context).Accounts.FirstOrDefaultAsync(x => x.DefaultUserProfileId == defaultUserProfileId);
        }

    }
}
