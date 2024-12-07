using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using InfinityNetServer.Services.Profile.Application.Exceptions;
using InfinityNetServer.Services.Profile.Application.IServices;
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
        IProfileService profileService, IMapper mapper) : ProfileService.ProfileServiceBase
    {

        public override async Task<PreviewFriendsResponse> getPreviewFriends(ProfilesRequest request, ServerCallContext context)
        {
            logger.LogInformation("GetFriendsOfProfile");
            var source = await userProfileService.GetAllByIds(request.Ids);
            var response = new PreviewFriendsResponse();
            response.Friends.AddRange(source.Select(mapper.Map<UserProfileResponse>).ToList());
            return response;
        }

        public override async Task<ProfileIdsResponse> getProfileIds(Empty request, ServerCallContext context)
        {
            logger.LogInformation("Received get profile ids request");
            var response = new ProfileIdsResponse();
            var profiles = await profileService.GetAll();
            response.Ids.AddRange(profiles.Select(p => p.Id.ToString()).ToList());

            return await Task.FromResult(response);
        }

        public override async Task<ProfileIdsResponse> getUserProfileIds(Empty request, ServerCallContext context)
        {
            logger.LogInformation("Received get user profile ids request");
            var response = new ProfileIdsResponse();
            var userProfiles = await profileService.GetAllByType(BuildingBlocks.Domain.Enums.ProfileType.User);
            response.Ids.AddRange(userProfiles.Select(p => p.Id.ToString()).ToList());

            return await Task.FromResult(response);
        }

        public override async Task<ProfileIdsResponse> getPageProfileIds(Empty request, ServerCallContext context)
        {
            logger.LogInformation("Received get page profile ids request");
            var response = new ProfileIdsResponse();
            var pageProfiles = await profileService.GetAllByType(BuildingBlocks.Domain.Enums.ProfileType.Page);
            response.Ids.AddRange(pageProfiles.Select(p => p.Id.ToString()).ToList());

            return await Task.FromResult(response);
        }

        public override async Task<ProfileResponse> getProfile(ProfileRequest request, ServerCallContext context)
        {
            logger.LogInformation("GetProfile called with ProfileId: {ProfileId}", request.Id);
            var source = await profileService.GetById(request.Id);
            string name = source.Type switch
            {
                BuildingBlocks.Domain.Enums.ProfileType.User => source.UserProfile.FirstName + " " + (source.UserProfile.MiddleName != null ? source.UserProfile.MiddleName + " " : "") + source.UserProfile.LastName,
                BuildingBlocks.Domain.Enums.ProfileType.Page => source.PageProfile.Name,
                _ => throw new ProfileException(ProfileError.INVALID_PROFILE_TYPE, StatusCodes.Status404NotFound),
            };
            source.AvatarId ??= Guid.Empty;
            source.CoverId ??= Guid.Empty;
            source.Location ??= string.Empty;
            var response = mapper.Map<ProfileResponse>(source);
            response.Name = name;
            return response;
        }

        public override async Task<UserProfileResponse> getUserProfile(ProfileRequest request, ServerCallContext context)
        {
            logger.LogInformation("GetUserProfile called with ProfileId: {ProfileId}", request.Id);
            var source = await userProfileService.GetById(request.Id);
            source.AvatarId ??= Guid.Empty;
            source.CoverId ??= Guid.Empty;
            source.MiddleName ??= string.Empty;
            source.Location ??= string.Empty;
            return mapper.Map<UserProfileResponse>(source);
        }

        public override async Task<ProfileIdsWithNamesResponse> getProfileIdsWithNames(ProfilesRequest request, ServerCallContext context)
        {
            logger.LogInformation("Received get profile ids with name request");
            var response = new ProfileIdsWithNamesResponse();
            var profiles = await profileService.GetAllByIds(request.Ids);
            response.ProfileIdsWithNames.AddRange(profiles.Select(p => new ProfileIdWithName
            {
                Id = p.Id.ToString(),
                Name = p.Type == BuildingBlocks.Domain.Enums.ProfileType.User
                ? p.UserProfile.FirstName + " " + (p.UserProfile.MiddleName != null ? p.UserProfile.MiddleName + " " : "") + p.UserProfile.LastName
                : p.PageProfile.Name
            }).ToList());

            return await Task.FromResult(response);
        }

        public override async Task<ProfileIdsResponse> getPotentialProfileIds(PotentialProfilesRequest request, ServerCallContext context)
        {
            logger.LogInformation("Received get potential profiles request");
            var response = new ProfileIdsResponse();
            var profiles = await profileService.GetPotentialByLocation(request.Location, request.Limit);
            response.Ids.AddRange(profiles.Select(p => p.Id.ToString()).ToList());

            return await Task.FromResult(response);
        }

        public override async Task<PreviewFileMetadatasResponse> getPreviewFileMetadatas(Empty request, ServerCallContext context)
        {
            logger.LogInformation("Received getFileMetadataIdsWithTypes request");
            var response = new PreviewFileMetadatasResponse();
            var comments = await profileService.GetAll();
            response.PreviewFileMetadatas.AddRange(
                 comments.SelectMany(p =>
                 {
                     var avatar = new PreviewFileMetadata
                     {
                         Id = p.Id.ToString(),
                         OwnerId = p.Id.ToString(),
                         FileMetadataId = p.AvatarId.ToString()
                     };
                     var cover = new PreviewFileMetadata
                     {
                         Id = p.Id.ToString(),
                         OwnerId = p.Id.ToString(),
                         FileMetadataId = p.CoverId.ToString()
                     };

                     // Trả về danh sách gồm avatar và cover
                     return new[] { avatar, cover };
                 })
             );

            return await Task.FromResult(response);
        }

        public override async Task<ProfilesResponse> getProfiles(ProfilesRequest request, ServerCallContext context)
        {
            logger.LogInformation("GetProfile called with Profiles");
            var source = await profileService.GetAllByIds(request.Ids);
            var profiles = source.Select(profile =>
            {
                profile.AvatarId ??= Guid.Empty;
                profile.CoverId ??= Guid.Empty;
                profile.Location ??= string.Empty;
                var result = mapper.Map<ProfileResponse>(profile);
                result.Name = profile.Type switch
                {
                    BuildingBlocks.Domain.Enums.ProfileType.User => profile.UserProfile.FirstName + " " + (profile.UserProfile.MiddleName != null ? profile.UserProfile.MiddleName + " " : "") + profile.UserProfile.LastName,
                    BuildingBlocks.Domain.Enums.ProfileType.Page => profile.PageProfile.Name,
                    _ => throw new ProfileException(ProfileError.INVALID_PROFILE_TYPE, StatusCodes.Status404NotFound),
                }; ;
                return result;
            }).ToList();

            var response = new ProfilesResponse();
            response.Profiles.AddRange(profiles);
            return response;
        }

    }
}
