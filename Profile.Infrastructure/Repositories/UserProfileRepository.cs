using System;
using InfinityNetServer.Services.Profile.Infrastructure.Data;
using InfinityNetServer.Services.Profile.Domain.Entities;
using InfinityNetServer.Services.Profile.Domain.Repositories;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace InfinityNetServer.Services.Profile.Infrastructure.Repositories
{
    public class UserProfileRepository : SqlRepository<UserProfile, Guid>, IUserProfileRepository
    {
        
        public UserProfileRepository(ProfileDbContext context) : base(context)
        { }

        public async Task<UserProfile> GetUserProfileByAccountIdAsync(Guid accountId)
        {
            return await ((ProfileDbContext)_context).UserProfiles.FirstOrDefaultAsync(profile => profile.AccountId == accountId);
        }
    }
}
