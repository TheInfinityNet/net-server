using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using InfinityNetServer.Services.Relationship.Domain.Repositories;
using InfinityNetServer.Services.Relationship.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Relationship.Infrastructure.Repositories
{
    public class ProfileFollowRepository(RelationshipDbContext context) 
        : SqlRepository<ProfileFollow, Guid>(context), IProfileFollowRepository
    {

        public async Task<ProfileFollow> GetByFollowerIdAndFolloweeIdAsync(Guid followerId, Guid followeeId)
            => await context.ProfileFollows.FirstOrDefaultAsync(i => i.FollowerId == followerId && i.FolloweeId == followeeId);


        public async Task<IList<Guid>> GetAllFolloweeIdsAsync(Guid profileId, int? limit)
        {
            var query = context.ProfileFollows.Where(i => i.FollowerId == profileId).Select(i => i.FolloweeId);

            if (limit.HasValue) query = query.Take(limit.Value);

            return await query.ToListAsync();
        }

        public async Task<IList<Guid>> GetAllFollowerIdsAsync(Guid profileId, int? limit)
        {
            var query = context.ProfileFollows.Where(i => i.FolloweeId == profileId).Select(i => i.FollowerId);

            if (limit.HasValue) query = query.Take(limit.Value);

            return await query.ToListAsync();
        }

    }
}
