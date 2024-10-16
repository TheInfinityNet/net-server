using Grpc.Core;
using Microsoft.Extensions.Logging;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using System.Threading.Tasks;
using InfinityNetServer.Services.Profile.Application.Interfaces;
using AutoMapper;

namespace InfinityNetServer.Services.Profile.Application.GrpcServices
{
    public class GrpcProfileService : ProfileService.ProfileServiceBase
    {

        private readonly ILogger<GrpcProfileService> _logger;

        private readonly IUserProfileService _userProfileService;

        private readonly IMapper _mapper;

        public GrpcProfileService(ILogger<GrpcProfileService> logger, IUserProfileService userProfileService, IMapper mapper)
        {
            _logger = logger;
            _userProfileService = userProfileService;
            _mapper = mapper;
        }

        public override async Task<UserProfileResponse> getUserProfile(GetProfileRequest request, ServerCallContext context)
        {
            _logger.LogInformation("GetUserProfile called with ProfileId: {ProfileId}", request.Id);
            var source = await _userProfileService.GetUserProfileById(request.Id);
            return _mapper.Map<UserProfileResponse>(source);
        }

    }
}
