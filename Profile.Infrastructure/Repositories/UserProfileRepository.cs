using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Profile.Domain.Entities;
using InfinityNetServer.Services.Profile.Domain.Repositories;
using InfinityNetServer.Services.Profile.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Profile.Infrastructure.Repositories
{
    public class UserProfileRepository(ProfileDbContext context) : SqlRepository<UserProfile, Guid>(context), IUserProfileRepository
    {
        public async Task<UserProfile> GetByAccountIdAsync(Guid accountId)
            => await DbSet.FirstOrDefaultAsync(profile => profile.AccountId == accountId);

        public async Task<IList<UserProfile>> GetAllByIdsAsync(IEnumerable<Guid> ids)
            => await DbSet.Where(profile => ids.Contains(profile.Id)).ToListAsync();
    }
}
