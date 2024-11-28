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

        public override async Task<ProfileIdsResponse> getFriendIds(ProfileRequest request, ServerCallContext context)
        {
            logger.LogInformation("Get friends for ProfileId: {ProfileId}", request.Id);
            var source = await friendshipService.GetFriendIds(request.Id);
            var response = new ProfileIdsResponse();
            response.Ids.AddRange(source);
            return response;
        }

        public override async Task<ProfileIdsResponse> getFollowerIds(ProfileRequest request, ServerCallContext context)
        {
            logger.LogInformation("Get followers for ProfileId: {ProfileId}", request.Id);
            var source = await profileFollowService.GetAllFollowerIds(request.Id);
            var response = new ProfileIdsResponse();
            response.Ids.AddRange(source);
            return response;
        }

        public override async Task<ProfileIdsResponse> getFolloweeIds(ProfileRequest request, ServerCallContext context)
        {
            logger.LogInformation("Get followers for ProfileId: {ProfileId}", request.Id);
            var source = await profileFollowService.GetAllFolloweeIds(request.Id);
            var response = new ProfileIdsResponse();
            response.Ids.AddRange(source);
            return response;
        }

        public override async Task<ProfileIdsResponse> getBlockerIds(ProfileRequest request, ServerCallContext context)
        {
            logger.LogInformation("Get blockers for ProfileId: {ProfileId}", request.Id);
            var source = await profileBlockService.GetBlockerIds(request.Id);
            var response = new ProfileIdsResponse();
            response.Ids.AddRange(source);
            return response;
        }

        public override async Task<ProfileIdsResponse> getBlockeeIds(ProfileRequest request, ServerCallContext context)
        {
            logger.LogInformation("Get blockees for ProfileId: {ProfileId}", request.Id);
            var source = await profileBlockService.GetBlockeeIds(request.Id);
            var response = new ProfileIdsResponse();
            response.Ids.AddRange(source);
            return response;
        }

    }
}
