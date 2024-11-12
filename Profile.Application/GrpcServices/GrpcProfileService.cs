using Grpc.Core;
using Microsoft.Extensions.Logging;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using System.Threading.Tasks;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using InfinityNetServer.Services.Profile.Domain.Repositories;
using System.Linq;
using InfinityNetServer.Services.Profile.Application.Services;
using System;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Requests;
using InfinityNetServer.Services.Profile.Application.Exceptions;
using Microsoft.AspNetCore.Http;

namespace InfinityNetServer.Services.Profile.Application.GrpcServices
{
    public class GrpcProfileService
        (ILogger<GrpcProfileService> logger, 
        IUserProfileService userProfileService, 
        IProfileRepository profileRepository, IMapper mapper) : ProfileService.ProfileServiceBase
    {

        public override async Task<PreviewFriendsOfProfileResponse> getPreviewFriendsOfProfile(GetPreviewFriendsOfProfileRequest request, ServerCallContext context)
        {
            logger.LogInformation("GetFriendsOfProfile");
            var source = await userProfileService.GetUserProfilesByIds(request.FriendIds);
            var response = new PreviewFriendsOfProfileResponse();
            response.Friends.AddRange(source.Select(mapper.Map<UserProfileResponse>).ToList());
            return response;
        }

        public override async Task<GetProfileIdsResponse> getProfileIds(Empty request, ServerCallContext context)
        {
            logger.LogInformation("Received get profile ids request");
            var response = new GetProfileIdsResponse();
            var profiles = await profileRepository.GetAllAsync();
            response.Ids.AddRange(profiles.Select(p => p.Id.ToString()).ToList());

            return await Task.FromResult(response);
        }

        public override async Task<GetUserProfileIdsResponse> getUserProfileIds(Empty request, ServerCallContext context)
        {
            logger.LogInformation("Received get user profile ids request");
            var response = new GetUserProfileIdsResponse();
            var userProfiles = await profileRepository.GetByType(BuildingBlocks.Domain.Enums.ProfileType.User);
            response.Ids.AddRange(userProfiles.Select(p => p.Id.ToString()).ToList());

            return await Task.FromResult(response);
        }

        public override async Task<GetPageProfileIdsResponse> getPageProfileIds(Empty request, ServerCallContext context)
        {
            logger.LogInformation("Received get page profile ids request");
            var response = new GetPageProfileIdsResponse();
            var pageProfiles = await profileRepository.GetByType(BuildingBlocks.Domain.Enums.ProfileType.Page);
            response.Ids.AddRange(pageProfiles.Select(p => p.Id.ToString()).ToList());

            return await Task.FromResult(response);
        }

        public override async Task<ProfileResponse> getProfile(GetProfileRequest request, ServerCallContext context)
        {
            logger.LogInformation("GetProfile called with ProfileId: {ProfileId}", request.Id);
            var source = await profileRepository.GetByIdAsync(Guid.Parse(request.Id));
            string name = source.Type switch
            {
                BuildingBlocks.Domain.Enums.ProfileType.User => source.UserProfile.FirstName + " " + (source.UserProfile.MiddleName != null ? source.UserProfile.MiddleName + " " : "") + source.UserProfile.LastName,
                BuildingBlocks.Domain.Enums.ProfileType.Page => source.PageProfile.Name,
                _ => throw new ProfileException(ProfileErrorCode.PROFILE_TYPE_NOT_FOUND, StatusCodes.Status404NotFound),
            };
            if (source.AvatarId == null) source.AvatarId = Guid.Empty;
            if (source.CoverId == null) source.CoverId = Guid.Empty;
            var response = mapper.Map<ProfileResponse>(source);
            response.Name = name;
            return response;
        }

        public override async Task<UserProfileResponse> getUserProfile(GetProfileRequest request, ServerCallContext context)
        {
            logger.LogInformation("GetUserProfile called with ProfileId: {ProfileId}", request.Id);
            var source = await userProfileService.GetUserProfileById(request.Id);
            if (source.AvatarId == null) source.AvatarId = Guid.Empty;
            if (source.CoverId == null) source.CoverId = Guid.Empty;
            return mapper.Map<UserProfileResponse>(source);
        }

        public override async Task<GetProfileIdsWithNamesResponse> getProfileIdsWithNames(Empty request, ServerCallContext context)
        {
            logger.LogInformation("Received get profile ids with name request");
            var response = new GetProfileIdsWithNamesResponse();
            var profiles = await profileRepository.GetAllAsync();
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
