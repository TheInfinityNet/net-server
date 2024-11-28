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


        public async Task<Friendship> GetByStatus(FriendshipStatus status, Guid senderId, Guid receiverId)
            => await context.Friendships.FirstOrDefaultAsync(f =>
                    f.SenderId.Equals(senderId) && f.ReceiverId.Equals(receiverId) && f.Status == status);

        //public async Task<IList<Friendship>> GetAllMyFriendInvitationsAsync(Guid profile, int? limit)
        //{
        //    var query = context.Friendships.Where(f => f.ReceiverId == profile && f.Status == FriendshipStatus.Pending);
        //    if (limit.HasValue) query = query.Take(limit.Value);
        //    return await query.ToListAsync();
        //}

        //public async Task<IList<Friendship>> GetAllSentFriendInvitationsAsync(Guid profile, int? limit)
        //{
        //    var query = context.Friendships.Where(f => f.SenderId == profile && f.Status == FriendshipStatus.Pending);
        //    if (limit.HasValue) query = query.Take(limit.Value);
        //    return await query.ToListAsync();
        //}

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

        public async Task<int> GetMutualFriendsCount(Guid profileId, Guid currentProfile)
            => await context.Friendships
                .CountAsync(f => (f.SenderId == profileId || f.ReceiverId == profileId) &&
                            (f.SenderId == currentProfile || f.ReceiverId == currentProfile) &&
                            f.Status == FriendshipStatus.Connected);

        public async Task<IList<Guid>> GetFriendsOfCurrentUserAsync(Guid? currentUserId)
        {
            return await context.Friendships
                .Where(f => (f.SenderId == currentUserId || f.ReceiverId == currentUserId) && f.Status != FriendshipStatus.NotConnected)
                .Select(f => f.SenderId == currentUserId ? f.ReceiverId : f.SenderId)
                .ToListAsync();
        }

        public async Task<IList<Guid>> GetMutualFriendsAsync(Guid? currentUserId, IList<Guid> friendsOfCurrentUser)
        {
            return await context.Friendships
                .Where(f => friendsOfCurrentUser.Contains(f.SenderId) || friendsOfCurrentUser.Contains(f.ReceiverId))
                .Where(f => f.Status == FriendshipStatus.Connected)
                .Where(f => f.SenderId != currentUserId && f.ReceiverId != currentUserId)
                .Select(f => f.SenderId == currentUserId ? f.ReceiverId : f.SenderId)
                .Distinct()
                .ToListAsync();
        }
        public async Task<IQueryable<Friendship>> GetMutualFriendsQueryAsync(Guid? currentUserId, IList<Guid> friendsOfCurrentUser, Guid? cursor)
        {
            IQueryable<Friendship> query = context.Friendships
                .Where(f => friendsOfCurrentUser.Contains(f.SenderId) || friendsOfCurrentUser.Contains(f.ReceiverId))
                .Where(f => f.Status == FriendshipStatus.Connected)
                .Where(f => f.SenderId != currentUserId && f.ReceiverId != currentUserId);

            if (cursor.HasValue)
            {
                query = query.Where(f => EF.Property<Guid>(f, "Id").CompareTo(cursor.Value) > 0);
            }

            return query;
        }
        public async Task<IList<Friendship>> GetPagedResultsAsync(IQueryable<Friendship> query, int pageSize)
        {
            var results = await query.Take(pageSize + 1).ToListAsync();
            return results;
        }
        public (bool hasNext, bool hasPrevious, IList<Friendship> results) ProcessPagedResults(IList<Friendship> results, int pageSize, Guid? cursor)
        {
            bool hasNext = results.Count > pageSize;
            bool hasPrevious = cursor != null;

            if (hasNext)
            {
                results.RemoveAt(results.Count - 1);  // Loại bỏ phần tử thừa nếu có trang tiếp theo
            }

            return (hasNext, hasPrevious, results);
        }
        public IList<Guid> GetCommonFriendsIds(IList<Friendship> results, Guid? currentUserId)
        {
            return results
                .Select(f => f.SenderId == currentUserId ? f.ReceiverId : f.SenderId)
                .Distinct()
                .ToList();
        }

        //public async Task<IList<(Guid friendId, int commonFriendsCount)>> GetCommonFriendsWithCount(IList<Guid> commonFriendsIds, IList<Friendship> friendships)
        //{
        //    return commonFriendsIds.Select(friendId => new
        //    {
        //        FriendId = friendId,
        //        CommonFriendsCount = friendships.Count(f => (f.SenderId == friendId || f.ReceiverId == friendId))
        //    })
        //    .Select(item => (item.FriendId, item.CommonFriendsCount))  // Chuyển sang dạng Tuple
        //    .ToList();
        //}
    }
}
