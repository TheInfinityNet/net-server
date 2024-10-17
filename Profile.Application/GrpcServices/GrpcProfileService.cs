using Grpc.Core;
using Microsoft.Extensions.Logging;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using System.Threading.Tasks;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using InfinityNetServer.Services.Profile.Domain.Repositories;
using System.Linq;
using InfinityNetServer.Services.Profile.Application.Services;

namespace InfinityNetServer.Services.Profile.Application.GrpcServices
{
    public class GrpcProfileService : ProfileService.ProfileServiceBase
    {

        private readonly ILogger<GrpcProfileService> _logger;

        private readonly IUserProfileService _userProfileService;

        private readonly IProfileRepository _profileRepository;

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

        public override async Task<GetProfileIdsResponse> getProfileIds(Empty request, ServerCallContext context)
        {
            _logger.LogInformation("Received getAccountIds request");
            var response = new GetProfileIdsResponse();
            var posts = await _profileRepository.GetAllAsync();
            response.ProfileIds.AddRange(posts.Select(p => p.Id.ToString()).ToList());

            return await Task.FromResult(response);
        }

    }
}
