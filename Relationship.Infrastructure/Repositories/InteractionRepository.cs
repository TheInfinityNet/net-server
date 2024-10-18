using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using InfinityNetServer.Services.Relationship.Domain.Repositories;
using InfinityNetServer.Services.Relationship.Infrastructure.Data;
using System;

namespace InfinityNetServer.Services.Relationship.Infrastructure.Repositories
{
    public class InteractionRepository : SqlRepository<Interaction, Guid>, IInteractionRepository
    {
        public InteractionRepository(RelationshipDbContext context) : base(context) { }




    }
}
