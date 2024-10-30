using System;
using InfinityNetServer.Services.Profile.Infrastructure.Data;
using InfinityNetServer.Services.Profile.Domain.Entities;
using InfinityNetServer.Services.Profile.Domain.Repositories;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace InfinityNetServer.Services.Profile.Infrastructure.Repositories
{
    public class UserProfileRepository(ProfileDbContext context) : SqlRepository<UserProfile, Guid>(context), IUserProfileRepository
    {
        public async Task<UserProfile> GetUserProfileByAccountIdAsync(Guid accountId)
            => await ((ProfileDbContext)context).UserProfiles.FirstOrDefaultAsync(profile => profile.AccountId == accountId);

        public async Task<IList<UserProfile>> GetUserProfilesByIdsAsync(IEnumerable<Guid> ids)
            => await ((ProfileDbContext)context).UserProfiles.Where(profile => ids.Contains(profile.Id)).ToListAsync();
    }
}
