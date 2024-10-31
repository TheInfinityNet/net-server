using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using InfinityNetServer.BuildingBlocks.Application.Protos;

namespace InfinityNetServer.BuildingBlocks.Application.GrpcClients
{
    public class CommonRelationshipClient(RelationshipService.RelationshipServiceClient client, ILogger<CommonRelationshipClient> logger)
    {

        public async Task<bool> HasFriendship(string currentProfileId, string targetProfileId)
        {
            try
            {
                logger.LogInformation("Starting has friendship");
                var response = await client.hasFriendshipAsync(new GetProfilesRelationshipRequest
                {
                    CurrentProfileId = currentProfileId,
                    TargetProfileId = targetProfileId
                });
                // Call the gRPC server to introspect the token
                return response.Result;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new CommonException(BaseErrorCode.RELATIONSHIP_NOT_FOUND, StatusCodes.Status404NotFound);
            }
        }

        public async Task<bool> HasBlocked(string currentProfileId, string targetProfileId)
        {
            try
            {
                logger.LogInformation("Starting has blocked");
                var response = await client.hasBlockedAsync(new GetProfilesRelationshipRequest
                {
                    CurrentProfileId = currentProfileId,
                    TargetProfileId = targetProfileId
                });
                // Call the gRPC server to introspect the token
                return response.Result;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new CommonException(BaseErrorCode.RELATIONSHIP_NOT_FOUND, StatusCodes.Status404NotFound);
            }
        }

        public async Task<bool> HasFollowed(string currentProfileId, string targetProfileId)
        {
            try
            {
                logger.LogInformation("Starting has followed");
                var response = await client.hasFollowedAsync(new GetProfilesRelationshipRequest
                {
                    CurrentProfileId = currentProfileId,
                    TargetProfileId = targetProfileId
                });
                // Call the gRPC server to introspect the token
                return response.Result;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new CommonException(BaseErrorCode.RELATIONSHIP_NOT_FOUND, StatusCodes.Status404NotFound);
            }
        }

        public async Task<bool> HasMuted(string currentProfileId, string targetProfileId)
        {
            try
            {
                logger.LogInformation("Starting has muted");
                var response = await client.hasMutedAsync(new GetProfilesRelationshipRequest
                {
                    CurrentProfileId = currentProfileId,
                    TargetProfileId = targetProfileId
                });
                // Call the gRPC server to introspect the token
                return response.Result;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new CommonException(BaseErrorCode.RELATIONSHIP_NOT_FOUND, StatusCodes.Status404NotFound);
            }
        }

        public async Task<bool> HasFriendRequest(string currentProfileId, string targetProfileId)
        {
            try
            {
                logger.LogInformation("Starting has friend request");
                var response = await client.hasFriendRequestAsync(new GetProfilesRelationshipRequest
                {
                    CurrentProfileId = currentProfileId,
                    TargetProfileId = targetProfileId
                });
                // Call the gRPC server to introspect the token
                return response.Result;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new CommonException(BaseErrorCode.RELATIONSHIP_NOT_FOUND, StatusCodes.Status404NotFound);
            }
        }

    }
}
