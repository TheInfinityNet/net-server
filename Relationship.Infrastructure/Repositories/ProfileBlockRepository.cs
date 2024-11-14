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
    public class ProfileBlockRepository(RelationshipDbContext context) 
        : SqlRepository<ProfileBlock, Guid>(context), IProfileBlockRepository
    {

        public async Task<ProfileBlock> GetByBlockerIdAndBlockeeId(Guid blockerId, Guid blockeeId)
            => await context.ProfileBlocks.FirstOrDefaultAsync(i => i.BlockerId == blockerId && i.BlockeeId == blockeeId);

        public async Task<IList<Guid>> GetAllBlockeeIdsAsync(Guid profileId, int? limit)
        {
            var query = context.ProfileBlocks.Where(i => i.BlockerId == profileId).Select(i => i.BlockeeId);

            if (limit.HasValue) query = query.Take(limit.Value);

            return await query.ToListAsync();
        }

        public async Task<IList<Guid>> GetAllBlockerIdsAsync(Guid profileId, int? limit)
        {
            var query = context.ProfileBlocks.Where(i => i.BlockeeId == profileId).Select(i => i.BlockerId);

            if (limit.HasValue) query = query.Take(limit.Value);

            return await query.ToListAsync();
        }

    }
}
