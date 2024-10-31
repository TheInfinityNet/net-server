using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using InfinityNetServer.Services.Relationship.Domain.Enums;
using InfinityNetServer.Services.Relationship.Domain.Repositories;
using InfinityNetServer.Services.Relationship.Infrastructure.Data;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace InfinityNetServer.Services.Relationship.Infrastructure.Repositories
{
    public class InteractionRepository(RelationshipDbContext context) : SqlRepository<Interaction, Guid>(context), IInteractionRepository
    {

        public async Task<Interaction> GetByType(InteractionType type, Guid profileId, Guid relateProfileId)
            => await context.Interactions
                .FirstOrDefaultAsync(i => 
                    i.Type == type 
                       && i.ProfileId == profileId 
                       && i.RelateProfileId == relateProfileId);


    }
}
