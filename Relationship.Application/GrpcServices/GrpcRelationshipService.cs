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

        public override async Task<GetProfilesRelationshipResponse> hasFriendship(GetProfilesRelationshipRequest request, ServerCallContext context)
        {
            logger.LogInformation("ProfileId: {ProfileId} has friendship with ProfileId: {ProfileId}", request.CurrentProfileId, request.TargetProfileId);
            var source = await friendshipService.HasFriendship(request.CurrentProfileId, request.TargetProfileId, FriendshipStatus.Connected);
            var response = new GetProfilesRelationshipResponse
            {
                Result = source
            };
            return response;
        }

        public override async Task<GetProfilesRelationshipResponse> hasBlocked(GetProfilesRelationshipRequest request, ServerCallContext context)
        {
            logger.LogInformation("ProfileId: {ProfileId} has blocked ProfileId: {ProfileId}", request.CurrentProfileId, request.TargetProfileId);
            var source = await profileBlockService.HasBlocked(request.CurrentProfileId, request.TargetProfileId);
            var response = new GetProfilesRelationshipResponse
            {
                Result = source
            };
            return response;
        }

        public override async Task<GetProfilesRelationshipResponse> hasFollowed(GetProfilesRelationshipRequest request, ServerCallContext context)
        {
            logger.LogInformation("ProfileId: {ProfileId} has followed ProfileId: {ProfileId}", request.CurrentProfileId, request.TargetProfileId);
            var source = await profileFollowService.HasFollowed(request.CurrentProfileId, request.TargetProfileId);
            var response = new GetProfilesRelationshipResponse
            {
                Result = source
            };
            return response;
        }

        public override async Task<GetProfilesRelationshipResponse> hasFriendRequest(GetProfilesRelationshipRequest request, ServerCallContext context)
        {
            logger.LogInformation("ProfileId: {ProfileId} has friend request from ProfileId: {ProfileId}", request.CurrentProfileId, request.TargetProfileId);
            var source = await friendshipService.HasFriendship(request.CurrentProfileId, request.TargetProfileId, FriendshipStatus.Pending);
            var response = new GetProfilesRelationshipResponse
            {
                Result = source
            };
            return response;
        }

        public override async Task<ProfileInteractionIdsResponse> getFollowerIds(GetProfileInteractionIdsRequest request, ServerCallContext context)
        {
            logger.LogInformation("Get followers for ProfileId: {ProfileId}", request.ProfileId);
            var source = await profileFollowService.GetAllFollowerIds(request.ProfileId, null);
            var response = new ProfileInteractionIdsResponse();
            response.ProfileInteractionIds.AddRange(source);
            return response;
        }

    }
}
