using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using System;
using System.Threading.Tasks;
using InfinityNetServer.Services.Relationship.Domain.Enums;
using System.Collections.Generic;

namespace InfinityNetServer.Services.Relationship.Domain.Repositories
{
    public interface IInteractionRepository : ISqlRepository<Interaction, Guid>
    {

        Task<Interaction> GetByTypeAsync(InteractionType type, Guid profileId, Guid relateProfileId);

        Task<IList<Interaction>> GetByProfileAndTypeAsync(InteractionType type, Guid profileId, int? limit);

        Task<IList<Interaction>> GetByRelateProfileAndTypeAsync(InteractionType type, Guid relateProfileId, int? limit);

    }
}
