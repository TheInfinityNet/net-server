using System;
using InfinityNetServer.Services.Relationship.Application;
using System.Threading.Tasks;
using InfinityNetServer.Services.Relationship.Domain.Repositories;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using InfinityNetServer.Services.Relationship.Application.Services;
using InfinityNetServer.Services.Relationship.Domain.Enums;
using InfinityNetServer.Services.Relationship.Application.DTOs.Responses;
using InfinityNetServer.BuildingBlocks.Domain.Repositories;

namespace InfinityNetServer.Services.Relationship.Presentation.Services
{
    public class FriendshipService(
        IFriendshipRepository friendshipRepository, 
        ILogger<FriendshipService> logger,
        IStringLocalizer<RelationshipSharedResource> localizer) : IFriendshipService
    {

        public async Task<bool> HasFriendship(string senderId, string receiverId, FriendshipStatus status)
            => await friendshipRepository.HasFriendship(Guid.Parse(senderId), Guid.Parse(receiverId), status);

        public async Task<Friendship> GetByStatus(FriendshipStatus status, string senderId, string receiverId)
            => await friendshipRepository.GetByStatus(status, Guid.Parse(senderId), Guid.Parse(receiverId));

        public async Task<int> CountFriendships(string profileId)
            => await friendshipRepository.CountFriendshipsAsync(Guid.Parse(profileId));

        public async Task<IList<string>> GetPreviewFriendIds(string profileId)
        {
            IList<Friendship> friendships = await friendshipRepository
                .GetFriendshipsWithLimitAsync(Guid.Parse(profileId), 10);

            return friendships.Select(f => 
                f.SenderId.ToString() == profileId 
                    ? f.ReceiverId.ToString() : f.SenderId.ToString()).ToList();
        }

        public async Task<PagedCursorResult<Guid>> GetPagedCommonFriendsAsync(Guid? currentUserId, Guid? cursor, int pageSize)
        {
            var friendsOfCurrentUser = await friendshipRepository.GetFriendsOfCurrentUserAsync(currentUserId);

            IQueryable<Friendship> query = await friendshipRepository.GetMutualFriendsQueryAsync(currentUserId, friendsOfCurrentUser, cursor);

            var results = await friendshipRepository.GetPagedResultsAsync(query, pageSize);

            var (hasNext, hasPrevious, pagedResults) = friendshipRepository.ProcessPagedResults(results, pageSize, cursor);

            var commonFriendsIds = friendshipRepository.GetCommonFriendsIds(pagedResults, currentUserId);

            string nextCursor = hasNext ? pagedResults.LastOrDefault()?.GetType().GetProperty("Id")?.GetValue(pagedResults.LastOrDefault()).ToString() : null;
            string previousCursor = hasPrevious ? cursor?.ToString() : null;

            return new PagedCursorResult<Guid>
            {
                Results = commonFriendsIds,
                NextCursor = nextCursor,
                PreviousCursor = previousCursor,
                HasNext = hasNext,
                HasPrevious = hasPrevious
            };
        }
    }
}
