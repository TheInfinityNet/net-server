using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using InfinityNetServer.Services.Identity.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Application.GrpcClients
{
    public class ProfileClient(ProfileService.ProfileServiceClient client, ILogger<ProfileClient> logger, IMapper mapper)
    {

        public async Task<BuildingBlocks.Application.DTOs.Responses.Profile.UserProfileResponse> GetUserProfile(string id)
        {
            try
            {
                logger.LogInformation("Starting get user profile");
                var response = await client.getUserProfileAsync(new GetProfileRequest
                {
                    Id = id
                });
                return mapper.Map<BuildingBlocks.Application.DTOs.Responses.Profile.UserProfileResponse>(response);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new IdentityException(IdentityErrorCode.USER_NOT_FOUND, StatusCodes.Status404NotFound);
            }
        }

    }
}
