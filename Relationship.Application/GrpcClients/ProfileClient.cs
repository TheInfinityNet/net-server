using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using InfinityNetServer.Services.Relationship.Application.Exceptions;

namespace InfinityNetServer.Services.Relationship.Application.GrpcClients
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

        public async Task<IList<BuildingBlocks.Application.DTOs.Responses.UserProfileResponse>> GetFriendsOfProfile(IList<string> ids)
        {
            try
            {
                _logger.LogInformation("Starting get friends of profile");
                var response = await _client.getFriendsOfProfileAsync(new GetFriendsOfProfileRequest
                {
                    Ids = { ids }
                });
                return response.Friends.Select(_mapper.Map<BuildingBlocks.Application.DTOs.Responses.UserProfileResponse>).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new RelationshipException(RelationshipErrorCode.FRIENDS_NOT_FOUND, StatusCodes.Status404NotFound);
            }
        }

    }
}
