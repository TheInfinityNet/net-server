using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Relationship.Domain.Repositories
{
    public interface IProfileBlockRepository : ISqlRepository<ProfileBlock, Guid>
    {

        Task<ProfileBlock> GetByBlockerIdAndBlockeeId(Guid blockerId, Guid blockeeId);

        Task<IList<Guid>> GetAllBlockeeIdsAsync(Guid profileId); // get all profiles that was blocked by profileId

        Task<IList<Guid>> GetAllBlockerIdsAsync(Guid profileId); // get all profiles that was blocked by profileId

    }
}
