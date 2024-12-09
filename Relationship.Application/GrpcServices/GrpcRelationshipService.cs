using AutoMapper;
using Grpc.Core;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using InfinityNetServer.Services.Relationship.Application.IServices;
using InfinityNetServer.Services.Relationship.Domain.Enums;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Relationship.Application.GrpcServices
{
    public class GrpcRelationshipService(
        ILogger<GrpcRelationshipService> logger,
        IFriendshipService friendshipService,
        IProfileFollowService profileFollowService,
        IProfileBlockService profileBlockService,
        IMapper mapper) : RelationshipService.RelationshipServiceBase
    {

        public override async Task<ProfileRelationshipResponse> hasFriendship(ProfilesRelationshipRequest request, ServerCallContext context)
        {
            logger.LogInformation("ProfileId: {ProfileId} has friendship with ProfileId: {ProfileId}", request.CurrentProfileId, request.TargetProfileId);
            var source = await friendshipService.HasFriendship(request.CurrentProfileId, request.TargetProfileId, FriendshipStatus.Connected);
            var response = new ProfileRelationshipResponse
            {
                Result = source
            };
            return response;
        }

        public override async Task<ProfileRelationshipResponse> hasBlocked(ProfilesRelationshipRequest request, ServerCallContext context)
        {
            logger.LogInformation("ProfileId: {ProfileId} has blocked ProfileId: {ProfileId}", request.CurrentProfileId, request.TargetProfileId);
            var source = await profileBlockService.HasBlocked(request.CurrentProfileId, request.TargetProfileId);
            var response = new ProfileRelationshipResponse
            {
                Result = source
            };
            return response;
        }

        public override async Task<ProfileRelationshipResponse> hasFollowed(ProfilesRelationshipRequest request, ServerCallContext context)
        {
            logger.LogInformation("ProfileId: {ProfileId} has followed ProfileId: {ProfileId}", request.CurrentProfileId, request.TargetProfileId);
            var source = await profileFollowService.HasFollowed(request.CurrentProfileId, request.TargetProfileId);
            var response = new ProfileRelationshipResponse
            {
                Result = source
            };
            return response;
        }

        public override async Task<ProfileRelationshipResponse> hasFriendRequest(ProfilesRelationshipRequest request, ServerCallContext context)
        {
            logger.LogInformation("ProfileId: {ProfileId} has friend request from ProfileId: {ProfileId}", request.CurrentProfileId, request.TargetProfileId);
            var source = await friendshipService.HasFriendship(request.CurrentProfileId, request.TargetProfileId, FriendshipStatus.Pending);
            var response = new ProfileRelationshipResponse
            {
                Result = source
            };
            return response;
        }

        public override async Task<ProfileIdsResponse> getAllFriendIds(ProfileRequest request, ServerCallContext context)
        {
            logger.LogInformation("Get friends for ProfileId: {ProfileId}", request.Id);
            var source = await friendshipService.GetAllFriendIds(request.Id);
            var response = new ProfileIdsResponse();
            response.Ids.AddRange(source);
            return response;
        }

        public override async Task<ProfileIdsResponse> getAllPendingRequestIds(ProfileRequest request, ServerCallContext context)
        {
            logger.LogInformation("Get pending request profiles of ProfileId: {ProfileId}", request.Id);
            var source = await friendshipService.GetAllPendingRequestIds(request.Id);
            var response = new ProfileIdsResponse();
            response.Ids.AddRange(source);
            return response;
        }

        public override async Task<ProfileIdsResponse> getAllRequestIds(ProfileRequest request, ServerCallContext context)
        {
            logger.LogInformation("Get friend requests profiles of ProfileId: {ProfileId}", request.Id);
            var source = await friendshipService.GetAllRequestIds(request.Id);
            var response = new ProfileIdsResponse();
            response.Ids.AddRange(source);
            return response;
        }

        public override async Task<ProfileIdsResponse> getAllSentRequestIds(ProfileRequest request, ServerCallContext context)
        {
            logger.LogInformation("Get friend requests profiles of ProfileId: {ProfileId}", request.Id);
            var source = await friendshipService.GetAllSentRequestIds(request.Id);
            var response = new ProfileIdsResponse();
            response.Ids.AddRange(source);
            return response;
        }

        public override async Task<ProfileIdsResponse> getAllFollowerIds(ProfileRequest request, ServerCallContext context)
        {
            logger.LogInformation("Get followers for ProfileId: {ProfileId}", request.Id);
            var source = await profileFollowService.GetAllFollowerIds(request.Id);
            var response = new ProfileIdsResponse();
            response.Ids.AddRange(source);
            return response;
        }

        public override async Task<ProfileIdsResponse> getAllFolloweeIds(ProfileRequest request, ServerCallContext context)
        {
            logger.LogInformation("Get followers for ProfileId: {ProfileId}", request.Id);
            var source = await profileFollowService.GetAllFolloweeIds(request.Id);
            var response = new ProfileIdsResponse();
            response.Ids.AddRange(source);
            return response;
        }

        public override async Task<ProfileIdsResponse> getAllBlockerIds(ProfileRequest request, ServerCallContext context)
        {
            logger.LogInformation("Get blockers for ProfileId: {ProfileId}", request.Id);
            var source = await profileBlockService.GetAllBlockerIds(request.Id);
            var response = new ProfileIdsResponse();
            response.Ids.AddRange(source);
            return response;
        }
        
        public override async Task<ProfileIdsResponse> getAllBlockeeIds(ProfileRequest request, ServerCallContext context)
        {
            logger.LogInformation("Get blockees for ProfileId: {ProfileId}", request.Id);
            var source = await profileBlockService.GetAllBlockeeIds(request.Id);
            var response = new ProfileIdsResponse();
            response.Ids.AddRange(source);
            return response;
        }
        public override async Task<MutualFriendCountResponse> getFriendsOfMutualFriends(ProfileRequest request, ServerCallContext context)
        {
            logger.LogInformation("Get mutual friend count for ProfileId: {ProfileId}", request.Id);
            var source = await friendshipService.GetAllMutualFriendsWithCount(request.Id);
            var response = new MutualFriendCountResponse();
            response.ProfileIdsWithMutualCounts.AddRange(source.Select(mapper.Map<ProfileIdWithMutualCount>));
            return response;
        }
        public override async Task<MutualFriendCountResponse> countMutualFriends(MutualFriendCountRequest request, ServerCallContext context)
        {
            logger.LogInformation("Get mutual friend count for ProfileId: {ProfileId}", request.CurrentProfileId);
            var source = await friendshipService.CountMutualFriends(request.CurrentProfileId, request.ProfileIds);
            var response = new MutualFriendCountResponse();
            response.ProfileIdsWithMutualCounts.AddRange(source.Select(mapper.Map<ProfileIdWithMutualCount>));
            return response;
        }
    }
}
