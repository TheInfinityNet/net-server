using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using InfinityNetServer.Services.Relationship.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Relationship.Domain.Repositories
{
    public interface IFriendshipRepository : ISqlRepository<Friendship, Guid>
    {

        Task<bool> HasFriendship(Guid currentProfileId, Guid targetProfileId, FriendshipStatus status);

        Task<Friendship> GetByStatus(FriendshipStatus status, Guid senderId, Guid receiverId);

        Task<Friendship> HasFriendship(Guid senderId, Guid receiverId);

        Task<IList<Guid>> GetAllFriendIdsAsync(Guid profile);

        Task<IList<Guid>> GetAllPendingRequestIdsAsync(Guid currentUserId);

        Task<int> CountFriendshipsAsync(Guid profileId);

        Task<IList<Friendship>> GetAllFriendshipsAsync(Guid profileId, int? limit);

        Task<int> CountMutualFriends(Guid profileId, Guid currentProfile);

        Task<IList<Guid>> GetAllMutualFriendIdsAsync(Guid currentUserId, IList<Guid> friendsOfCurrentUser);

        Task<IList<(Guid MutualFriendId, int MutualFriendCount)>> GetMutualFriendsAndCount(Guid currentUserId, IList<Guid> friendsOfMutualFriend);

        Task<IList<Guid>> GetFriendsOfMutualFriend(Guid currentUserId);

        Task<IList<(Guid FriendId, int MutualFriendCount)>> CountMutualFriends(IList<string> results, Guid currentUserId);

        Task<IList<Guid>> GetAllRequestIdsAsync(Guid currentUserId);

        Task<IList<Guid>> GetAllSentRequestIdsAsync(Guid currentUserId);

    }
}
