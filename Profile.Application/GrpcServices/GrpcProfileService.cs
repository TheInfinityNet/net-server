using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using InfinityNetServer.Services.Profile.Application.Exceptions;
using InfinityNetServer.Services.Profile.Application.Services;
using InfinityNetServer.Services.Profile.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Profile.Application.GrpcServices
{
    public class GrpcProfileService
        (ILogger<GrpcProfileService> logger, 
        IUserProfileService userProfileService, 
        IProfileRepository profileRepository, IMapper mapper) : ProfileService.ProfileServiceBase
    {

        public override async Task<PreviewFriendsResponse> getPreviewFriends(ProfilesRequest request, ServerCallContext context)
        {
            logger.LogInformation("GetFriendsOfProfile");
            var source = await userProfileService.GetUserProfilesByIds(request.Ids);
            var response = new PreviewFriendsResponse();
            response.Friends.AddRange(source.Select(mapper.Map<UserProfileResponse>).ToList());
            return response;
        }

        public override async Task<ProfileIdsResponse> getProfileIds(Empty request, ServerCallContext context)
        {
            logger.LogInformation("Received get profile ids request");
            var response = new ProfileIdsResponse();
            var profiles = await profileRepository.GetAllAsync();
            response.Ids.AddRange(profiles.Select(p => p.Id.ToString()).ToList());

            return await Task.FromResult(response);
        }

        public override async Task<ProfileIdsResponse> getUserProfileIds(Empty request, ServerCallContext context)
        {
            logger.LogInformation("Received get user profile ids request");
            var response = new ProfileIdsResponse();
            var userProfiles = await profileRepository.GetByType(BuildingBlocks.Domain.Enums.ProfileType.User);
            response.Ids.AddRange(userProfiles.Select(p => p.Id.ToString()).ToList());

            return await Task.FromResult(response);
        }

        public override async Task<ProfileIdsResponse> getPageProfileIds(Empty request, ServerCallContext context)
        {
            logger.LogInformation("Received get page profile ids request");
            var response = new ProfileIdsResponse();
            var pageProfiles = await profileRepository.GetByType(BuildingBlocks.Domain.Enums.ProfileType.Page);
            response.Ids.AddRange(pageProfiles.Select(p => p.Id.ToString()).ToList());

            return await Task.FromResult(response);
        }

        public override async Task<ProfileResponse> getProfile(ProfileRequest request, ServerCallContext context)
        {
            logger.LogInformation("GetProfile called with ProfileId: {ProfileId}", request.Id);
            var source = await profileRepository.GetByIdAsync(Guid.Parse(request.Id));
            string name = source.Type switch
            {
                BuildingBlocks.Domain.Enums.ProfileType.User => source.UserProfile.FirstName + " " + (source.UserProfile.MiddleName != null ? source.UserProfile.MiddleName + " " : "") + source.UserProfile.LastName,
                BuildingBlocks.Domain.Enums.ProfileType.Page => source.PageProfile.Name,
                _ => throw new ProfileException(ProfileErrorCode.PROFILE_TYPE_NOT_FOUND, StatusCodes.Status404NotFound),
            };
            source.AvatarId ??= Guid.Empty;
            source.CoverId ??= Guid.Empty;
            var response = mapper.Map<ProfileResponse>(source);
            response.Name = name;
            return response;
        }

        public override async Task<UserProfileResponse> getUserProfile(ProfileRequest request, ServerCallContext context)
        {
            logger.LogInformation("GetUserProfile called with ProfileId: {ProfileId}", request.Id);
            var source = await userProfileService.GetUserProfileById(request.Id);
            source.AvatarId ??= Guid.Empty;
            source.CoverId ??= Guid.Empty;
            return mapper.Map<UserProfileResponse>(source);
        }

        public override async Task<ProfileIdsWithNamesResponse> getProfileIdsWithNames(ProfilesRequest request, ServerCallContext context)
        {
            logger.LogInformation("Received get profile ids with name request");
            var response = new ProfileIdsWithNamesResponse();
            var profiles = await profileRepository.GetByIdsAsync(request.Ids.Select(Guid.Parse).ToList());
            response.ProfileIdsWithNames.AddRange(profiles.Select(p => new ProfileIdWithName
            {
                Id = p.Id.ToString(),
                Name = p.Type == BuildingBlocks.Domain.Enums.ProfileType.User
                ? p.UserProfile.FirstName + " " + (p.UserProfile.MiddleName != null ? p.UserProfile.MiddleName + " " : "") + p.UserProfile.LastName
                : p.PageProfile.Name
            }).ToList());

            return await Task.FromResult(response);
        }

    }
}
