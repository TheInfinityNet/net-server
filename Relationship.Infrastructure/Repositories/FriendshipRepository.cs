using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using InfinityNetServer.Services.Relationship.Domain.Enums;
using InfinityNetServer.Services.Relationship.Domain.Repositories;
using InfinityNetServer.Services.Relationship.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Relationship.Infrastructure.Repositories
{
    public class FriendshipRepository(RelationshipDbContext context) : SqlRepository<Friendship, Guid>(context), IFriendshipRepository
    {

        public async Task<bool> HasFriendship(Guid currentProfileId, Guid targetProfileId, FriendshipStatus status)
            => await context.Friendships.AnyAsync(f =>
                ((f.SenderId.Equals(currentProfileId) && f.ReceiverId.Equals(targetProfileId)) ||
                (f.SenderId.Equals(targetProfileId) && f.ReceiverId.Equals(currentProfileId))) && f.Status.Equals(status));

        public async Task<int> CountFriendshipsAsync(Guid profileId)
            => await context.Friendships.CountAsync(f =>
                    (f.SenderId == profileId || f.ReceiverId == profileId) &&
                    f.Status == FriendshipStatus.Connected);

        public async Task<Friendship> HasFriendship(Guid senderId, Guid receiverId)
        => await context.Friendships.FirstOrDefaultAsync(f =>
                    (f.SenderId.Equals(senderId) && f.ReceiverId.Equals(receiverId)) || (f.SenderId.Equals(receiverId) && f.ReceiverId.Equals(senderId)));

        public async Task<Friendship> GetByStatus(FriendshipStatus status, Guid senderId, Guid receiverId)
            => await context.Friendships.FirstOrDefaultAsync(f =>
                    f.SenderId.Equals(senderId) && f.ReceiverId.Equals(receiverId) && f.Status == status);

        public async Task<IList<Guid>> GetAllFriendIdsAsync(Guid profile)
            => await context.Friendships
                .Where(f => (f.SenderId == profile || f.ReceiverId == profile) && f.Status == FriendshipStatus.Connected)
                .Select(f => f.SenderId == profile ? f.ReceiverId : f.SenderId).ToListAsync();

        public async Task<IList<Friendship>> GetAllFriendshipsAsync(Guid profileId, int? limit)
            => await context.Friendships
                .Where(f =>
                    (f.SenderId == profileId || f.ReceiverId == profileId)
                    && f.Status == FriendshipStatus.Connected)
                .Take(limit ?? context.Friendships.Count()).ToListAsync();

        public async Task<int> CountMutualFriends(Guid profileId, Guid currentProfile)
            => await context.Friendships
                .CountAsync(f => (f.SenderId == profileId || f.ReceiverId == profileId) &&
                            (f.SenderId == currentProfile || f.ReceiverId == currentProfile) &&
                            f.Status == FriendshipStatus.Connected);

        public async Task<IList<Guid>> GetAllMutualFriendIdsAsync(Guid currentUserId, IList<Guid> friendsOfCurrentUser)
        {
            return await context.Friendships
                .Where(f => friendsOfCurrentUser.Contains(f.SenderId) || friendsOfCurrentUser.Contains(f.ReceiverId))
                .Where(f => f.Status == FriendshipStatus.Connected)
                .Where(f => f.SenderId != currentUserId && f.ReceiverId != currentUserId)
                .Select(f => f.SenderId == currentUserId ? f.ReceiverId : f.SenderId)
                .Distinct()
                .ToListAsync();
        }
        
        public async Task<IList<Guid>> GetAllFriendIdsOfCurrentUserAsync(Guid currentUserId)
        {
            return await context.Friendships
                .Where(f => (f.SenderId == currentUserId || f.ReceiverId == currentUserId) && f.Status == FriendshipStatus.Connected )
                .Select(f => f.SenderId == currentUserId ? f.ReceiverId : f.SenderId)
                .ToListAsync();
        }

        public async Task<IList<Guid>> GetAllPendingRequestIdsAsync(Guid currentUserId)
        {
            return await context.Friendships
                .Where(f => (f.SenderId == currentUserId || f.ReceiverId == currentUserId) && f.Status == FriendshipStatus.Pending)
                .Select(f => f.SenderId == currentUserId ? f.ReceiverId : f.SenderId)
                .ToListAsync();
        }

        public async Task<IList<Guid>> GetAllRequestIdsAsync(Guid currentUserId)
        {
            return await context.Friendships
                .Where(f => (f.ReceiverId == currentUserId) && f.Status == FriendshipStatus.Pending)
                .Select(f => f.SenderId)
                .ToListAsync();
        }

        public async Task<IList<Guid>> GetAllSentRequestIdsAsync(Guid currentUserId)
        {
            return await context.Friendships
                .Where(f => (f.SenderId == currentUserId) && f.Status == FriendshipStatus.Pending)
                .Select(f => f.ReceiverId)
                .ToListAsync();
        }

        public async Task<IList<(Guid FriendId, int MutualFriendCount)>> CountMutualFriends(IList<string> results, Guid currentUserId)
        {
            var resultGuids = results.Select(Guid.Parse).ToList();
            var queryResult = await context.Friendships
                .Where(f => resultGuids.Contains(f.SenderId) || resultGuids.Contains(f.ReceiverId)) 
                .Where(f => f.SenderId != currentUserId && f.ReceiverId != currentUserId) 
                .Select(f => f.SenderId == currentUserId ? f.ReceiverId : f.SenderId)
                .GroupBy(friendId => friendId) 
                .Select(group => new 
                {
                    FriendId = group.Key,
                    Count = group.Count()
                })
                .ToListAsync(); 

            return queryResult.Select(x => (x.FriendId, x.Count)).ToList();
        }

    }
}
