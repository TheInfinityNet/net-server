using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using System;
using System.Threading.Tasks;
using InfinityNetServer.Services.Relationship.Domain.Enums;

namespace InfinityNetServer.Services.Relationship.Domain.Repositories
{
    public interface IInteractionRepository : ISqlRepository<Interaction, Guid>
    {

        Task<Interaction> GetByType(InteractionType type, Guid profileId, Guid relateProfileId);

    }
}
