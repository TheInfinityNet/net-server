using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using InfinityNetServer.Services.Identity.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Application.GrpcClients
{
    public class ProfileClient
    {

        private readonly ProfileService.ProfileServiceClient _client;

        private readonly ILogger<ProfileClient> _logger;

        private readonly IMapper _mapper;

        public ProfileClient(ProfileService.ProfileServiceClient client, ILogger<ProfileClient> logger, IMapper mapper)
        {
            _client = client;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<BuildingBlocks.Application.DTOs.Responses.UserProfileResponse> GetUserProfile(string id)
        {
            try
            {
                _logger.LogInformation("Starting get user profile");
                var response = await _client.getUserProfileAsync(new GetProfileRequest
                {
                    Id = id
                });
                return _mapper.Map<BuildingBlocks.Application.DTOs.Responses.UserProfileResponse>(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new IdentityException(IdentityErrorCode.USER_NOT_FOUND, StatusCodes.Status404NotFound);
            }
        }

    }
}
