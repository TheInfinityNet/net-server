﻿using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.Contracts.Commands;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Others;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.Services.Relationship.Application.DTOs.Responses;
using InfinityNetServer.Services.Relationship.Application.IServices;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using InfinityNetServer.Services.Relationship.Domain.Enums;
using InfinityNetServer.Services.Relationship.Domain.Repositories;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Relationship.Application.Services
{
    public class FriendshipService(
        IFriendshipRepository friendshipRepository,
        IProfileBlockService blockService,
        ILogger<FriendshipService> logger,
        IMessageBus messageBus,
        IStringLocalizer<RelationshipSharedResource> localizer) : IFriendshipService
    {

        public async Task<bool> HasFriendship(string senderId, string receiverId, FriendshipStatus status)
            => await friendshipRepository.HasFriendship(Guid.Parse(senderId), Guid.Parse(receiverId), status);

        public async Task<Friendship> HasFriendship(string senderId, string receiverId)
            => await friendshipRepository.HasFriendship(Guid.Parse(senderId), Guid.Parse(receiverId));

        public async Task<Friendship> GetByStatus(FriendshipStatus status, string senderId, string receiverId)
            => await friendshipRepository.GetByStatus(status, Guid.Parse(senderId), Guid.Parse(receiverId));

        public async Task<IList<string>> GetAllFriendIds(string profile)
        {
            var list = await friendshipRepository.GetAllFriendIdsAsync(Guid.Parse(profile));
            return list.Select(x => x.ToString()).ToList();
        }

        //public async Task<PagedCursorResult<FriendSuggestionResponse>> GetPagedCommonFriendsAsync(Guid? currentUserId, Guid? cursor, int pageSize)
        //{
        //    var friendsOfCurrentUser = await friendshipRepository.GetFriendsOfCurrentUserAsync(currentUserId);

        //    IQueryable<Friendship> query = await friendshipRepository.GetMutualFriendsQueryAsync(currentUserId, friendsOfCurrentUser, cursor);

        //    var results = await friendshipRepository.GetPagedResultsAsync(query, pageSize);

        //    var (hasNext, hasPrevious, pagedResults) = friendshipRepository.ProcessPagedResults(results, pageSize, cursor);

        //    var mutualFriendsWithCount = await friendshipRepository.GetFriendSuggestions(pagedResults, currentUserId);
        //    var profileIds = mutualFriendsWithCount
        //       .Select(item => item.FriendId)
        //       .Distinct();
        //    var profiles = await profileClient.GetProfiles(profileIds.Select(id => id.ToString()).ToList());
        //    var profileDict = profiles.ToDictionary(p => p.Id);

        //    var friendSuggestions = mutualFriendsWithCount.Select(mutualFriend => new FriendSuggestionResponse
        //    {
        //        Id = mutualFriend.FriendId,
        //        MutualFriendsCount = mutualFriend.MutualFriendCount,
        //        Name = (profileDict.TryGetValue(mutualFriend.FriendId, out var profile)) ? profile.Name : "", 
        //        Avatar = profile?.Avatar, 
        //        Status = "NotConnected"
        //    }).ToList();
        //    string nextCursor = hasNext ? pagedResults.LastOrDefault()?.GetType().GetProperty("Id")?.GetValue(pagedResults.LastOrDefault()).ToString() : null;
        //    string previousCursor = hasPrevious ? cursor?.ToString() : null;

        //    return new PagedCursorResult<FriendSuggestionResponse>
        //    {
        //        Results = Task.FromResult<IList<FriendSuggestionResponse>>(friendSuggestions),
        //        NextCursor = nextCursor,
        //        PreviousCursor = previousCursor,
        //        HasNext = hasNext,
        //        HasPrevious = hasPrevious
        //    };
        //}

        public async Task<CursorPagedResult<Friendship>> GetFriendRequests(string profileId, string cursor, int limit)
        {
            IList<string> blockerIds = await blockService.GetAllBlockerIds(profileId);
            IList<string> blockeeIds = await blockService.GetAllBlockeeIds(profileId);

            var specification = new SpecificationWithCursor<Friendship>
            {
                Criteria = friendship =>
                        friendship.Status == FriendshipStatus.Pending
                        && friendship.ReceiverId.Equals(Guid.Parse(profileId))
                        && !blockerIds.Concat(blockeeIds).Contains(friendship.SenderId.ToString()),
                Cursor = cursor,
                Limit = limit
            };
            return await friendshipRepository.GetPagedAsync(specification);
        }

        public async Task<IList<string>> GetPreviewFriendIds(string profileId)
        {
            IList<Friendship> friendships = await friendshipRepository
                .GetAllFriendshipsAsync(Guid.Parse(profileId), 10);

            return friendships.Select(f =>
                f.SenderId.ToString() == profileId
                    ? f.ReceiverId.ToString() : f.SenderId.ToString()).ToList();
        }

        public async Task<CursorPagedResult<Friendship>> GetFriends(string profileId, string cursor, int limit)
        {
            IList<string> blockerIds = await blockService.GetAllBlockerIds(profileId);
            IList<string> blockeeIds = await blockService.GetAllBlockeeIds(profileId);

            var specification = new SpecificationWithCursor<Friendship>
            {
                Criteria = friendship =>
                        friendship.Status == FriendshipStatus.Connected
                        && (friendship.SenderId.Equals(Guid.Parse(profileId)) || friendship.ReceiverId.Equals(Guid.Parse(profileId)))
                        && !blockerIds.Concat(blockeeIds).Contains(friendship.SenderId.ToString())
                        && !blockerIds.Concat(blockeeIds).Contains(friendship.ReceiverId.ToString()),
                Cursor = cursor,
                Limit = limit
            };
            return await friendshipRepository.GetPagedAsync(specification);
        }

        public async Task<CursorPagedResult<Friendship>> GetFriendSentRequests(string profileId, string cursor, int limit)
        {
            IList<string> blockerIds = await blockService.GetAllBlockerIds(profileId);
            IList<string> blockeeIds = await blockService.GetAllBlockeeIds(profileId);

            var specification = new SpecificationWithCursor<Friendship>
            {
                Criteria = friendship =>
                        friendship.Status == FriendshipStatus.Pending
                        && friendship.SenderId.Equals(Guid.Parse(profileId))
                        && !blockerIds.Concat(blockeeIds).Contains(friendship.SenderId.ToString()),
                Cursor = cursor,
                Limit = limit
            };
            return await friendshipRepository.GetPagedAsync(specification);
        }

        public async Task<int> CountFriendships(string profileId)
            => await friendshipRepository.CountFriendshipsAsync(Guid.Parse(profileId));

        public async Task<SendRequestResponse> SendRequest(string senderId, string receiverId)
        {
            Friendship friendship = new()
            {
                SenderId = Guid.Parse(senderId),
                ReceiverId = Guid.Parse(receiverId),
                Status = FriendshipStatus.Pending
            };
            var rs = await friendshipRepository.CreateAsync(friendship);
            await PublishFriendshipNotificationCommands(friendship);
            return new SendRequestResponse

            {
                UserId = friendship.Id.ToString(),
                Message = localizer["Friend request sent"].ToString(),
                Status = "RequestSent"
            };
        }

        public async Task<RejectRequestResponse> DeclineRequest(Guid requestId)
        {
            await friendshipRepository.DeleteAsync(requestId);
            return new RejectRequestResponse
            {
                UserId = requestId.ToString(),
                Message = "Friend request rejected",
                Status = "NotConnected"
            };
        }

        public async Task<CancelRequestResponse> CancelRequest(Guid requestId)
        {
            await friendshipRepository.DeleteAsync(requestId);
            return new CancelRequestResponse
            {
                UserId = requestId.ToString(),
                Message = "Friend request canceled",
                Status = "NotConnected"
            };
        }

        public async Task<AcceptRequestResponse> AcceptRequest(Friendship entity)
        {
            entity.Status = FriendshipStatus.Connected;
            var friendship = await friendshipRepository.UpdateAsync(entity);
            await PublishFriendshipNotificationCommands(friendship);
            return new AcceptRequestResponse
            {
                UserId = entity.Id.ToString(),
                Message = "Friend request accepted",
                Status = "Connected"
            };
        }

        public async Task<UnfriendResponse> Unfriend(Guid friendshipId)
        {
            await friendshipRepository.DeleteAsync(friendshipId);
            return new UnfriendResponse
            {
                UserId = friendshipId.ToString(),
                Message = "Removed friend",
                Status = "NotConnected"
            };
        }

        public async Task<IList<string>> GetAllPendingRequestIds(string profileId)
        {
            var list = await friendshipRepository.GetAllPendingRequestIdsAsync(Guid.Parse(profileId));
            return list.Select(x => x.ToString()).ToList();
        }

        public async Task<IList<ProfileIdWithMutualCount>> CountMutualFriends(string currentProfileId, IList<string> profileIds)
        {
            var list = profileIds.Select(x => Guid.Parse(x)).ToList();
            var friendsOfMutualFriends = await friendshipRepository.GetMutualFriendsAndCount(Guid.Parse(currentProfileId), list);
            return friendsOfMutualFriends.Select(item => new ProfileIdWithMutualCount
            {
                ProfileId = item.MutualFriendId.ToString(),
                Count = item.MutualFriendCount
            }).ToList();
        }

        public async Task<IList<string>> GetAllRequestIds(string profile)
        {
            var list = await friendshipRepository.GetAllRequestIdsAsync(Guid.Parse(profile));
            return list.Select(x => x.ToString()).ToList();
        }

        public async Task<IList<string>> GetAllSentRequestIds(string profile)
        {
            var list = await friendshipRepository.GetAllSentRequestIdsAsync(Guid.Parse(profile));
            return list.Select(x => x.ToString()).ToList();
        }

        private async Task PublishFriendshipNotificationCommands(Friendship entity)
        {
            Guid id = entity.Id;
            Guid senderId = entity.SenderId;
            Guid receiverId = entity.ReceiverId;
            DateTime createdAt = entity.CreatedAt;

            var notificationCommand = new DomainCommand.CreateFriendshipNotificationCommand
            {
                FriendshipId = id,
                CreatedAt = createdAt
            };

            switch (entity.Status)
            {
                case FriendshipStatus.Pending:
                    // Friend invitation
                    notificationCommand.TriggeredBy = senderId.ToString();
                    notificationCommand.TargetProfileId = receiverId;
                    notificationCommand.Type = BuildingBlocks.Domain.Enums.NotificationType.FriendInvitation;
                    break;

                case FriendshipStatus.Connected:
                    // Friend invitation accepted
                    notificationCommand.TriggeredBy = receiverId.ToString();
                    notificationCommand.TargetProfileId = senderId;
                    notificationCommand.Type = BuildingBlocks.Domain.Enums.NotificationType.FriendInvitationAccepted;
                    break;

                default:
                    return; // Do nothing if the status doesn't match
            }

            await messageBus.Publish(notificationCommand);
        }
        public async Task<IList<ProfileIdWithMutualCount>> GetAllMutualFriendsWithCount(string profile)
        {
            
            var list = await friendshipRepository.GetFriendsOfMutualFriend(Guid.Parse(profile));
            var friendsOfMutualFriends = await friendshipRepository.GetMutualFriendsAndCount(Guid.Parse(profile), list);
            return friendsOfMutualFriends.Select(item => new ProfileIdWithMutualCount
            {
                ProfileId = item.MutualFriendId.ToString(),
                Count = item.MutualFriendCount
            }).ToList();
        }

        public async Task<IList<string>> GetAllMutualFriendIds(string profile, string friendId)
        {
            var list = await friendshipRepository.GetMutualFriends(Guid.Parse(profile), Guid.Parse(friendId));
            return list.Select(x => x.ToString()).ToList();
        }
    }
}
