using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.BuildingBlocks.Domain.Specifications;
using InfinityNetServer.Services.Relationship.Application;
using InfinityNetServer.Services.Relationship.Application.DTOs.Responses;
using InfinityNetServer.Services.Relationship.Application.GrpcClients;
using InfinityNetServer.Services.Relationship.Application.Services;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using InfinityNetServer.Services.Relationship.Domain.Enums;
using InfinityNetServer.Services.Relationship.Domain.Repositories;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static StackExchange.Redis.Role;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Others;

namespace InfinityNetServer.Services.Relationship.Presentation.Services
{
    public class FriendshipService(
        CommonProfileClient profileClient,
        IFriendshipRepository friendshipRepository, 
        ILogger<FriendshipService> logger,
        IProfileFollowService profileFollowService,
        IProfileBlockService profileBlockService,
        IStringLocalizer<RelationshipSharedResource> localizer) : IFriendshipService
    {

        public async Task<bool> HasFriendship(string senderId, string receiverId, FriendshipStatus status)
            => await friendshipRepository.HasFriendship(Guid.Parse(senderId), Guid.Parse(receiverId), status);

        public async Task<Friendship> HasFriendship(string senderId, string receiverId)
            => await friendshipRepository.HasFriendship(Guid.Parse(senderId), Guid.Parse(receiverId));

        public async Task<Friendship> GetByStatus(FriendshipStatus status, string senderId, string receiverId)
            => await friendshipRepository.GetByStatus(status, Guid.Parse(senderId), Guid.Parse(receiverId));

        public async Task<IList<string>> GetFriendIds(string profile)
        {
            var list = await friendshipRepository.GetAllFriendIdsAsync(Guid.Parse(profile));
            return list.Select(x => x.ToString()).ToList();
        }

        public async Task<int> CountFriendships(string profileId)
            => await friendshipRepository.CountFriendshipsAsync(Guid.Parse(profileId));

        public async Task<IList<string>> GetPreviewFriendIds(string profileId)
        {
            IList<Friendship> friendships = await friendshipRepository
                .GetAllFriendshipsAsync(Guid.Parse(profileId), 10);

            return friendships.Select(f =>
                f.SenderId.ToString() == profileId
                    ? f.ReceiverId.ToString() : f.SenderId.ToString()).ToList();
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

        public async Task<SendRequestResponse> SendRequest(string senderId, string receiverId)
        {
            Friendship friendship = new Friendship
            {
                SenderId = Guid.Parse(senderId),
                ReceiverId = Guid.Parse(receiverId),
                Status = FriendshipStatus.Pending
            };
            await friendshipRepository.CreateAsync(friendship);
            return new SendRequestResponse
            {
                UserId = friendship.Id,
                Message = "Friend request sent",
                Status = "RequestSent"
            };
        }
        public async Task<RejectRequestResponse> RejectRequest(Guid requestId)
        {
            await friendshipRepository.DeleteAsync(requestId);
            return new RejectRequestResponse
            {
                UserId = requestId,
                Message = "Friend request rejected",
                Status = "NotConnected"
            };
        }
        public async Task<CancelRequestResponse> CancelRequest(Guid requestId)
        {
            await friendshipRepository.DeleteAsync(requestId);
            return new CancelRequestResponse
            {
                UserId = requestId,
                Message = "Friend request canceled",
                Status = "NotConnected"
            };
        }
        public Task<AcceptRequestResponse> AcceptRequest(string senderId, string receiverId)
        {
            throw new NotImplementedException();
        }
        public Task<UnfriendResponse> Unfriend(string senderId, string receiverId)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<string>> GetPendingRequestProfiles(string profile)
        {
            var list = await friendshipRepository.GetPendingRequestsAsync(Guid.Parse(profile));
            return list.Select(x => x.ToString()).ToList();
        }

        public async Task<IList<ProfileIdWithMutualCount>> GetCountMutualFriend(string currentProfileId, IList<string> profileIds)
        {
            var list = await friendshipRepository.GetMutualFriendCount(profileIds, Guid.Parse(currentProfileId));
            return list.Select(item => new ProfileIdWithMutualCount
            {
                ProfileId = item.FriendId.ToString(),
                Count = item.MutualFriendCount
            }).ToList();
        }

        public async Task<IList<string>> GetRequests(string profile)
        {
            var list = await friendshipRepository.GetRequestsAsync(Guid.Parse(profile));
            return list.Select(x => x.ToString()).ToList();
        }

        public async Task<IList<string>> GetSentRequests(string profile)
        {
            var list = await friendshipRepository.GetSentRequestsAsync(Guid.Parse(profile));
            return list.Select(x => x.ToString()).ToList();
        }

        public async Task<UnfriendResponse> Unfriend(Guid friendshipId)
        {
            await friendshipRepository.DeleteAsync(friendshipId);
            return new UnfriendResponse
            {
                UserId = friendshipId,
                Message = "Friend request canceled",
                Status = "NotConnected"
            };
        }
    }
}
