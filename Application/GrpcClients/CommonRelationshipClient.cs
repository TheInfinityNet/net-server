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
                throw new BaseException(BaseError.RELATIONSHIP_NOT_FOUND, StatusCodes.Status404NotFound);
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
                throw new BaseException(BaseError.RELATIONSHIP_NOT_FOUND, StatusCodes.Status404NotFound);
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
                throw new BaseException(BaseError.RELATIONSHIP_NOT_FOUND, StatusCodes.Status404NotFound);
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
                throw new BaseException(BaseError.RELATIONSHIP_NOT_FOUND, StatusCodes.Status404NotFound);
            }
        }
        public async Task<IList<string>> GetFriendIds(string profileId)
        {
            try
            {
                logger.LogInformation("Starting get followers");
                var response = await client.getFriendIdsAsync(new ProfileRequest
                {
                    Id = profileId
                });
                // Call the gRPC server to introspect the token
                return response.Ids;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new BaseException(BaseError.RELATIONSHIP_NOT_FOUND, StatusCodes.Status404NotFound);
            }
        }
        public async Task<IList<string>> GetPendingRequestProfiles(string profileId)
        {
            try
            {
                logger.LogInformation("Starting get pending request profiles");
                var response = await client.getPendingRequestProfilesAsync(new ProfileRequest
                {
                    Id = profileId
                });
                // Call the gRPC server to introspect the token
                return response.Ids;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new BaseException(BaseError.RELATIONSHIP_NOT_FOUND, StatusCodes.Status404NotFound);
            }
        }
        public async Task<IList<string>> GetRequestsProfiles(string profileId)
        {
            try
            {
                logger.LogInformation("Starting get friend requests profiles");
                var response = await client.getRequestProfilesAsync(new ProfileRequest
                {
                    Id = profileId
                });
                // Call the gRPC server to introspect the token
                return response.Ids;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new BaseException(BaseError.RELATIONSHIP_NOT_FOUND, StatusCodes.Status404NotFound);
            }
        }
        public async Task<IList<string>> GetSentRequestProfiles(string profileId)
        {
            try
            {
                logger.LogInformation("Starting get friend sent requests profiles");
                var response = await client.getSentRequestProfilesAsync(new ProfileRequest
                {
                    Id = profileId
                });
                // Call the gRPC server to introspect the token
                return response.Ids;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new BaseException(BaseError.RELATIONSHIP_NOT_FOUND, StatusCodes.Status404NotFound);
            }
        }
        public async Task<IList<string>> GetFollowerIds(string profileId)
        {
            try
            {
                logger.LogInformation("Starting get followers");
                var response = await client.getFollowerIdsAsync(new ProfileRequest
                {
                    Id = profileId
                });
                // Call the gRPC server to introspect the token
                return response.Ids;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new BaseException(BaseError.RELATIONSHIP_NOT_FOUND, StatusCodes.Status404NotFound);
            }
        }

        public async Task<IList<string>> GetFolloweeIds(string profileId)
        {
            try
            {
                logger.LogInformation("Starting get followers");
                var response = await client.getFolloweeIdsAsync(new ProfileRequest
                {
                    Id = profileId
                });
                // Call the gRPC server to introspect the token
                return response.Ids;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new BaseException(BaseError.RELATIONSHIP_NOT_FOUND, StatusCodes.Status404NotFound);
            }
        }

        public async Task<IList<string>> GetBlockerIds(string profileId)
        {
            try
            {
                logger.LogInformation("Starting get followers");
                var response = await client.getBlockerIdsAsync(new ProfileRequest
                {
                    Id = profileId
                });
                // Call the gRPC server to introspect the token
                return response.Ids;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new BaseException(BaseError.RELATIONSHIP_NOT_FOUND, StatusCodes.Status404NotFound);
            }
        }

        public async Task<IList<string>> GetBlockeeIds(string profileId)
        {
            try
            {
                logger.LogInformation("Starting get followers");
                var response = await client.getBlockeeIdsAsync(new ProfileRequest
                {
                    Id = profileId
                });
                // Call the gRPC server to introspect the token
                return response.Ids;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new BaseException(BaseError.RELATIONSHIP_NOT_FOUND, StatusCodes.Status404NotFound);
            }
        }
        public async Task<IList<ProfileIdWithMutualCount>> GetMutualCount(string profileId, IList<string> profileIds)
        {
            try
            {
                logger.LogInformation("Starting get mutual friend count");
                var request = new MutualFriendCountRequest();
                request.CurrentProfileId = profileId;
                request.ProfileIds.AddRange(profileIds);
                var response = await client.getMutualFriendCountAsync(request);
                // Call the gRPC server to introspect the token
                return response.ProfileIdsWithMutualCounts;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new BaseException(BaseError.RELATIONSHIP_NOT_FOUND, StatusCodes.Status404NotFound);
            }
        }
    }
}
