using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using InfinityNetServer.Services.Relationship.Domain.Enums;
using InfinityNetServer.Services.Relationship.Domain.Repositories;
using InfinityNetServer.Services.Relationship.Infrastructure.Data;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace InfinityNetServer.Services.Relationship.Infrastructure.Repositories
{
    public class InteractionRepository(RelationshipDbContext context) 
        : SqlRepository<Interaction, Guid>(context), IInteractionRepository
    {

        public async Task<Interaction> GetByTypeAsync(InteractionType type, Guid profileId, Guid relateProfileId)
            => await context.Interactions.FirstOrDefaultAsync(i => 
            i.Type == type && i.ProfileId == profileId && i.RelateProfileId == relateProfileId);


        public async Task<IList<Interaction>> GetByProfileAndTypeAsync(
            InteractionType type, Guid profileId, int? limit)
        {
            var query = context.Interactions
                .Where(i => i.Type == type && i.ProfileId == profileId);

            if (limit.HasValue)
            {
                query = query.Take(limit.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<IList<Interaction>> GetByRelateProfileAndTypeAsync(
            InteractionType type, Guid relateProfileId, int? limit)
        {
            var query = context.Interactions
                .Where(i => i.Type == type && i.RelateProfileId == relateProfileId);

            if (limit.HasValue)
            {
                query = query.Take(limit.Value);
            }

            return await query.ToListAsync();
        }

    }
}
