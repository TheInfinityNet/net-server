using InfinityNetServer.BuildingBlocks.Application.DTOs.Others;
using InfinityNetServer.BuildingBlocks.Domain.Repositories;
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
        public Task<IList<string>> GetFriendIds(string profile);
        public Task<IList<string>> GetPendingRequestProfiles(string profile);
        public Task<IList<string>> GetRequests(string profile);
        public Task<IList<string>> GetSentRequests(string profile);
        public Task<int> CountFriendships(string profileId);
        public Task<IList<string>> GetPreviewFriendIds(string profileId);
        public Task<SendRequestResponse> SendRequest(string senderId, string receiverId);
        public Task<CancelRequestResponse> CancelRequest(Guid requestId);
        public Task<AcceptRequestResponse> AcceptRequest(string senderId, string receiverId);
        public Task<RejectRequestResponse> RejectRequest(Guid requestId);
        public Task<UnfriendResponse> Unfriend(Guid friendshipId);
        public Task<IList<ProfileIdWithMutualCount>> GetCountMutualFriend(string currentProfileId, IList<string> profileIds);
    }
}
