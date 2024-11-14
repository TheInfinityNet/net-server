using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using InfinityNetServer.Services.Relationship.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Relationship.Application.GrpcClients
{
    public class ProfileClient(ProfileService.ProfileServiceClient client, ILogger<ProfileClient> logger, IMapper mapper)
    {

        public async Task<IList<BuildingBlocks.Application.DTOs.Responses.Profile.UserProfileResponse>> GetPreviewFriendsOfProfile(IList<string> friendIds)
        {
            try
            {
                logger.LogInformation("Starting get friends of profile");
                var response = await client.getPreviewFriendsOfProfileAsync(new GetPreviewFriendsOfProfileRequest
                {
                    FriendIds = { friendIds }
                });
                return response.Friends.Select(mapper.Map<BuildingBlocks.Application.DTOs.Responses.Profile.UserProfileResponse>).ToList();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new RelationshipException(RelationshipErrorCode.FRIENDS_NOT_FOUND, StatusCodes.Status404NotFound);
            }
        }
    }
}
