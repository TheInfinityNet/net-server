using InfinityNetServer.BuildingBlocks.Application.DTOs.Others;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.Services.Relationship.Application.DTOs.Responses;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using InfinityNetServer.Services.Relationship.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Relationship.Application.Services
{
    public interface IFriendshipService
    {

        public Task<bool> HasFriendship(string senderId, string receiverId, FriendshipStatus status);

        public Task<Friendship> HasFriendship(string senderId, string receiverId);

        public Task<Friendship> GetByStatus(FriendshipStatus status, string senderId, string receiverId);

        public Task<IList<string>> GetAllFriendIds(string profile);

        public Task<IList<string>> GetAllPendingRequestIdsByProfileId(string profile);

        public Task<IList<string>> GetRequestIds(string profile);

        public Task<IList<string>> GetSentRequestIds(string profile);

        public Task<int> CountFriendships(string profileId);

        public Task<IList<string>> GetPreviewFriendIds(string profileId);

        public Task<CursorPagedResult<Friendship>> GetFriendRequests(string profileId, string cursor, int pageSize);

        public Task<CursorPagedResult<Friendship>> GetFriends(string profileId, string cursor, int pageSize);

        public Task<CursorPagedResult<Friendship>> GetFriendSentRequests(string profileId, string cursor, int pageSize);

        public Task<SendRequestResponse> SendRequest(string senderId, string receiverId);

        public Task<CancelRequestResponse> CancelRequest(Guid requestId);

        public Task<AcceptRequestResponse> AcceptRequest(Friendship friendship);

        public Task<RejectRequestResponse> DeclineRequest(Guid requestId);

        public Task<UnfriendResponse> Unfriend(Guid friendshipId);

        public Task<IList<ProfileIdWithMutualCount>> CountMutualFriend(string currentProfileId, IList<string> profileIds);

    }
}
