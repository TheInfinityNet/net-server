using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using System;

namespace InfinityNetServer.Services.Relationship.Domain.Repositories
{
    public interface IInteractionRepository : ISqlRepository<Interaction, Guid>
    {



    }
}
