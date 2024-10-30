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

        public GrpcProfileService(ILogger<GrpcProfileService> logger, IUserProfileService userProfileService, IProfileRepository profileRepository, IMapper mapper)
        {
            _logger = logger;
            _userProfileService = userProfileService;
            _profileRepository = profileRepository;
            _mapper = mapper;
        }

        public override async Task<UserProfileResponse> getUserProfile(GetProfileRequest request, ServerCallContext context)
        {
            _logger.LogInformation("GetUserProfile called with ProfileId: {ProfileId}", request.Id);
            var source = await _userProfileService.GetUserProfileById(request.Id);
            return _mapper.Map<UserProfileResponse>(source);
        }

        public override async Task<PreviewFriendsOfProfileResponse> getPreviewFriendsOfProfile(GetPreviewFriendsOfProfileRequest request, ServerCallContext context)
        {
            _logger.LogInformation("GetFriendsOfProfile");
            var source = await _userProfileService.GetUserProfilesByIds(request.FriendIds);
            var response = new PreviewFriendsOfProfileResponse();
            response.Friends.AddRange(source.Select(_mapper.Map<UserProfileResponse>).ToList());
            return response;
        }

        public override async Task<GetProfileIdsResponse> getProfileIds(Empty request, ServerCallContext context)
        {
            _logger.LogInformation("Received get profile ids request");
            var response = new GetProfileIdsResponse();
            var profiles = await _profileRepository.GetAllAsync();
            response.Ids.AddRange(profiles.Select(p => p.Id.ToString()).ToList());

            return await Task.FromResult(response);
        }

        public override async Task<GetUserProfileIdsResponse> getUserProfileIds(Empty request, ServerCallContext context)
        {
            _logger.LogInformation("Received get user profile ids request");
            var response = new GetUserProfileIdsResponse();
            var userProfiles = await _profileRepository.GetByType(BuildingBlocks.Domain.Enums.ProfileType.User);
            response.Ids.AddRange(userProfiles.Select(p => p.Id.ToString()).ToList());

            return await Task.FromResult(response);
        }

        public override async Task<GetPageProfileIdsResponse> getPageProfileIds(Empty request, ServerCallContext context)
        {
            _logger.LogInformation("Received get page profile ids request");
            var response = new GetPageProfileIdsResponse();
            var pageProfiles = await _profileRepository.GetByType(BuildingBlocks.Domain.Enums.ProfileType.Page);
            response.Ids.AddRange(pageProfiles.Select(p => p.Id.ToString()).ToList());

            return await Task.FromResult(response);
        }

    }
}
