using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Relationship.Domain.Repositories
{
    public interface IProfileFollowRepository : ISqlRepository<ProfileFollow, Guid>
    {

        Task<ProfileFollow> GetByFollowerIdAndFolloweeIdAsync(Guid followerId, Guid followeeId);

        Task<IList<Guid>> GetAllFolloweeIdsAsync(Guid profileId);

        Task<IList<Guid>> GetAllFollowerIdsAsync(Guid profileId);

    }
}
