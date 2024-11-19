using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using System.Collections.Generic;

namespace InfinityNetServer.BuildingBlocks.Application.GrpcClients
{
    public class CommonRelationshipClient(RelationshipService.RelationshipServiceClient client, ILogger<CommonRelationshipClient> logger)
    {

        public async Task<bool> HasFriendship(string currentProfileId, string targetProfileId)
        {
            try
            {
                logger.LogInformation("Starting has friendship");
                var response = await client.hasFriendshipAsync(new ProfilesRelationshipRequest
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
                var response = await client.hasBlockedAsync(new ProfilesRelationshipRequest
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
                var response = await client.hasFollowedAsync(new ProfilesRelationshipRequest
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
                var response = await client.hasFriendRequestAsync(new ProfilesRelationshipRequest
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

        public async Task<IList<string>> GetFriendIds(string profileId)
        {
            try
            {
                logger.LogInformation("Starting get followers");
                var response = await client.getFriendIdsAsync(new ProfileInteractionIdRequest
                {
                    ProfileId = profileId
                });
                // Call the gRPC server to introspect the token
                return response.ProfileInteractionIds;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new CommonException(BaseErrorCode.RELATIONSHIP_NOT_FOUND, StatusCodes.Status404NotFound);
            }
        }

        public async Task<IList<string>> GetFollowerIds(string profileId)
        {
            try
            {
                logger.LogInformation("Starting get followers");
                var response = await client.getFollowerIdsAsync(new ProfileInteractionIdRequest
                {
                    ProfileId = profileId
                });
                // Call the gRPC server to introspect the token
                return response.ProfileInteractionIds;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new CommonException(BaseErrorCode.RELATIONSHIP_NOT_FOUND, StatusCodes.Status404NotFound);
            }
        }

        public async Task<IList<string>> GetFolloweeIds(string profileId)
        {
            try
            {
                logger.LogInformation("Starting get followers");
                var response = await client.getFolloweeIdsAsync(new ProfileInteractionIdRequest
                {
                    ProfileId = profileId
                });
                // Call the gRPC server to introspect the token
                return response.ProfileInteractionIds;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new CommonException(BaseErrorCode.RELATIONSHIP_NOT_FOUND, StatusCodes.Status404NotFound);
            }
        }

        public async Task<IList<string>> GetBlockerIds(string profileId)
        {
            try
            {
                logger.LogInformation("Starting get followers");
                var response = await client.getBlockerIdsAsync(new ProfileInteractionIdRequest
                {
                    ProfileId = profileId
                });
                // Call the gRPC server to introspect the token
                return response.ProfileInteractionIds;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new CommonException(BaseErrorCode.RELATIONSHIP_NOT_FOUND, StatusCodes.Status404NotFound);
            }
        }

        public async Task<IList<string>> GetBlockeeIds(string profileId)
        {
            try
            {
                logger.LogInformation("Starting get followers");
                var response = await client.getBlockeeIdsAsync(new ProfileInteractionIdRequest
                {
                    ProfileId = profileId
                });
                // Call the gRPC server to introspect the token
                return response.ProfileInteractionIds;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new CommonException(BaseErrorCode.RELATIONSHIP_NOT_FOUND, StatusCodes.Status404NotFound);
            }
        }

    }
}
