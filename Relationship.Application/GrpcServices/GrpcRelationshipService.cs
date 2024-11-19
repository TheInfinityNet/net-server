using Grpc.Core;
using Microsoft.Extensions.Logging;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using System.Threading.Tasks;
using AutoMapper;
using InfinityNetServer.Services.Relationship.Application.Services;
using System.Linq;
using InfinityNetServer.Services.Relationship.Domain.Enums;

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

        public override async Task<ProfileInteractionIdsResponse> getFriendIds(ProfileInteractionIdRequest request, ServerCallContext context)
        {
            logger.LogInformation("Get friends for ProfileId: {ProfileId}", request.ProfileId);
            var source = await friendshipService.GetFriendIds(request.ProfileId, null);
            var response = new ProfileInteractionIdsResponse();
            response.ProfileInteractionIds.AddRange(source);
            return response;
        }

        public override async Task<ProfileInteractionIdsResponse> getFollowerIds(ProfileInteractionIdRequest request, ServerCallContext context)
        {
            logger.LogInformation("Get followers for ProfileId: {ProfileId}", request.ProfileId);
            var source = await profileFollowService.GetAllFollowerIds(request.ProfileId, null);
            var response = new ProfileInteractionIdsResponse();
            response.ProfileInteractionIds.AddRange(source);
            return response;
        }

        public override async Task<ProfileInteractionIdsResponse> getFolloweeIds(ProfileInteractionIdRequest request, ServerCallContext context)
        {
            logger.LogInformation("Get followers for ProfileId: {ProfileId}", request.ProfileId);
            var source = await profileFollowService.GetAllFolloweeIds(request.ProfileId, null);
            var response = new ProfileInteractionIdsResponse();
            response.ProfileInteractionIds.AddRange(source);
            return response;
        }

        public override async Task<ProfileInteractionIdsResponse> getBlockerIds(ProfileInteractionIdRequest request, ServerCallContext context)
        {
            logger.LogInformation("Get blockers for ProfileId: {ProfileId}", request.ProfileId);
            var source = await profileBlockService.GetBlockerIds(request.ProfileId, null);
            var response = new ProfileInteractionIdsResponse();
            response.ProfileInteractionIds.AddRange(source);
            return response;
        }

        public override async Task<ProfileInteractionIdsResponse> getBlockeeIds(ProfileInteractionIdRequest request, ServerCallContext context)
        {
            logger.LogInformation("Get blockees for ProfileId: {ProfileId}", request.ProfileId);
            var source = await profileBlockService.GetBlockeeIds(request.ProfileId, null);
            var response = new ProfileInteractionIdsResponse();
            response.ProfileInteractionIds.AddRange(source);
            return response;
        }

    }
}
