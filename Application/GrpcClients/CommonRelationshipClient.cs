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
        public async Task<IList<string>> GetAllFriendIds(string profileId)
        {
            try
            {
                logger.LogInformation("Starting get followers");
                var response = await client.getAllFriendIdsAsync(new ProfileRequest
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
        public async Task<IList<string>> GetAllPendingRequestIds(string profileId)
        {
            try
            {
                logger.LogInformation("Starting get pending request profiles");
                var response = await client.getAllPendingRequestIdsAsync(new ProfileRequest
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
        public async Task<IList<string>> GetAllRequestIds(string profileId)
        {
            try
            {
                logger.LogInformation("Starting get friend requests profiles");
                var response = await client.getAllRequestIdsAsync(new ProfileRequest
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
        public async Task<IList<string>> GetAllSentRequestIds(string profileId)
        {
            try
            {
                logger.LogInformation("Starting get friend sent requests profiles");
                var response = await client.getAllSentRequestIdsAsync(new ProfileRequest
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
        public async Task<IList<string>> GetAllFollowerIds(string profileId)
        {
            try
            {
                logger.LogInformation("Starting get followers");
                var response = await client.getAllFollowerIdsAsync(new ProfileRequest
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

        public async Task<IList<string>> GetAllFolloweeIds(string profileId)
        {
            try
            {
                logger.LogInformation("Starting get followers");
                var response = await client.getAllFolloweeIdsAsync(new ProfileRequest
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

        public async Task<IList<string>> GetAllBlockerIds(string profileId)
        {
            try
            {
                logger.LogInformation("Starting get followers");
                var response = await client.getAllBlockerIdsAsync(new ProfileRequest
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
        
        public async Task<IList<string>> GetAllBlockeeIds(string profileId)
        {
            try
            {
                logger.LogInformation("Starting get followers");
                var response = await client.getAllBlockeeIdsAsync(new ProfileRequest
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
        public async Task<IList<ProfileIdWithMutualCount>> GetFriendsOfMutualFriends(string profileId)
        {
            try
            {
                logger.LogInformation("Starting get mutual friend list");
                var response = await client.getFriendsOfMutualFriendsAsync(new ProfileRequest
                {
                    Id = profileId
                });
                // Call the gRPC server to introspect the token
                return response.ProfileIdsWithMutualCounts;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new BaseException(BaseError.RELATIONSHIP_NOT_FOUND, StatusCodes.Status404NotFound);
            }
        }
        public async Task<IList<ProfileIdWithMutualCount>> CountMutualFriends(string profileId, IList<string> profileIds)
        {
            try
            {
                logger.LogInformation("Starting get mutual friend count");
                var request = new MutualFriendCountRequest
                {
                    CurrentProfileId = profileId
                };
                request.ProfileIds.AddRange(profileIds);
                var response = await client.countMutualFriendsAsync(request);
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
