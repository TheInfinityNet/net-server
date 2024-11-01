using Grpc.Core;
using Microsoft.Extensions.Logging;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using System.Threading.Tasks;
using AutoMapper;
using InfinityNetServer.Services.Relationship.Application.Services;
using System.Linq;

namespace InfinityNetServer.Services.Relationship.Application.GrpcServices
{
    public class GrpcRelationshipService(
        ILogger<GrpcRelationshipService> logger, 
        IFriendshipService friendshipService,
        IInteractionService interactionService, 
        IMapper mapper) : RelationshipService.RelationshipServiceBase
    {

        public override async Task<GetProfilesRelationshipResponse> hasFriendship(GetProfilesRelationshipRequest request, ServerCallContext context)
        {
            logger.LogInformation("ProfileId: {ProfileId} has friendship with ProfileId: {ProfileId}", request.CurrentProfileId, request.TargetProfileId);
            var source = await friendshipService.HasFriendship(request.CurrentProfileId, request.TargetProfileId);
            var response = new GetProfilesRelationshipResponse
            {
                Result = source
            };
            return response;
        }

        public override async Task<GetProfilesRelationshipResponse> hasBlocked(GetProfilesRelationshipRequest request, ServerCallContext context)
        {
            logger.LogInformation("ProfileId: {ProfileId} has blocked ProfileId: {ProfileId}", request.CurrentProfileId, request.TargetProfileId);
            var source = await interactionService.HasBlocked(request.CurrentProfileId, request.TargetProfileId);
            var response = new GetProfilesRelationshipResponse
            {
                Result = source
            };
            return response;
        }

        public override async Task<GetProfilesRelationshipResponse> hasFollowed(GetProfilesRelationshipRequest request, ServerCallContext context)
        {
            logger.LogInformation("ProfileId: {ProfileId} has followed ProfileId: {ProfileId}", request.CurrentProfileId, request.TargetProfileId);
            var source = await interactionService.HasFollowed(request.CurrentProfileId, request.TargetProfileId);
            var response = new GetProfilesRelationshipResponse
            {
                Result = source
            };
            return response;
        }

        public override async Task<GetProfilesRelationshipResponse> hasMuted(GetProfilesRelationshipRequest request, ServerCallContext context)
        {
            logger.LogInformation("ProfileId: {ProfileId} has muted ProfileId: {ProfileId}", request.CurrentProfileId, request.TargetProfileId);
            var source = await interactionService.HasMuted(request.CurrentProfileId, request.TargetProfileId);
            var response = new GetProfilesRelationshipResponse
            {
                Result = source
            };
            return response;
        }

        public override async Task<GetProfilesRelationshipResponse> hasFriendRequest(GetProfilesRelationshipRequest request, ServerCallContext context)
        {
            logger.LogInformation("ProfileId: {ProfileId} has friend request from ProfileId: {ProfileId}", request.CurrentProfileId, request.TargetProfileId);
            var source = await interactionService.HasFriendRequest(request.CurrentProfileId, request.TargetProfileId);
            var response = new GetProfilesRelationshipResponse
            {
                Result = source
            };
            return response;
        }

        public override async Task<GetFollowersResponse> getFollowers(GetFollowersRequest request, ServerCallContext context)
        {
            logger.LogInformation("Get followers for ProfileId: {ProfileId}", request.ProfileId);
            var source = await interactionService.GetByType(Domain.Enums.InteractionType.Follow, null, request.ProfileId, null);
            var response = new GetFollowersResponse();
            response.FollowerIds.AddRange(source.Select(i => i.ProfileId.ToString()).ToList());
            return response;
        }

    }
}
