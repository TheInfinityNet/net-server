using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfinityNetServer.Services.Relationship.Domain.Enums;

namespace InfinityNetServer.Services.Relationship.Domain.Repositories
{
    public interface IFriendshipRepository : ISqlRepository<Friendship, Guid>
    {

        Task<bool> HasFriendship(Guid senderId, Guid receiverId);

        Task<Friendship> GetByStatus(FriendshipStatus status, Guid senderId, Guid receiverId);

        Task<int> CountFriendshipsAsync(Guid profileId);

        Task<IList<Friendship>> GetFriendshipsWithLimitAsync(Guid profileId, int? limit);

    }
}
