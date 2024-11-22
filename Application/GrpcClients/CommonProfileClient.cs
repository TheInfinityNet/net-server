using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static InfinityNetServer.BuildingBlocks.Application.Protos.ProfileService;

namespace InfinityNetServer.BuildingBlocks.Application.GrpcClients
{
    public class CommonProfileClient(ProfileServiceClient client, ILogger<CommonProfileClient> logger, IMapper mapper)
    {

        public async Task<IList<string>> GetProfileIds()
        {
            try
            {
                logger.LogInformation("Starting get profile ids");
                var response = await client.getProfileIdsAsync(new Empty());
                // Call the gRPC server to introspect the token
                return new List<string>(response.Ids);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new CommonException(BaseErrorCode.SEED_DATA_ERROR, StatusCodes.Status422UnprocessableEntity);
            }
        }

        public async Task<IList<string>> GetUserProfileIds()
        {
            try
            {
                logger.LogInformation("Starting get user profile ids");
                var response = await client.getUserProfileIdsAsync(new Empty());
                // Call the gRPC server to introspect the token
                return new List<string>(response.Ids);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new CommonException(BaseErrorCode.SEED_DATA_ERROR, StatusCodes.Status422UnprocessableEntity);
            }
        }
        public async Task<IList<string>> GetPageProfileIds()
        {
            try
            {
                logger.LogInformation("Starting get page profile ids");
                var response = await client.getPageProfileIdsAsync(new Empty());
                // Call the gRPC server to introspect the token
                return new List<string>(response.Ids);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new CommonException(BaseErrorCode.SEED_DATA_ERROR, StatusCodes.Status422UnprocessableEntity);
            }
        }

        public async Task<DTOs.Responses.Profile.BaseProfileResponse> GetProfile(string id)
        {
            try
            {
                logger.LogInformation("Starting get user profile");
                var response = await client.getProfileAsync(new ProfileRequest { Id = id });
                return mapper.Map<DTOs.Responses.Profile.BaseProfileResponse>(response);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new CommonException(BaseErrorCode.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);
            }
        }

        public async Task<DTOs.Responses.Profile.UserProfileResponse> GetUserProfile(string id)
        {
            try
            {
                logger.LogInformation("Starting get user profile");
                var response = await client.getUserProfileAsync(new ProfileRequest { Id = id });
                return mapper.Map<DTOs.Responses.Profile.UserProfileResponse>(response);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new CommonException(BaseErrorCode.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);
            }
        }

        public async Task<IList<DTOs.Others.ProfileIdWithName>> GetProfileIdsWithNames(IList<string> ids)
        {
            try
            {
                logger.LogInformation("Starting get profile ids with name");
                var request = new ProfilesRequest();
                request.Ids.AddRange(ids);
                var response = await client.getProfileIdsWithNamesAsync(request);
                // Call the gRPC server to introspect the token
                return new List<DTOs.Others.ProfileIdWithName>(response.ProfileIdsWithNames
                    .Select(mapper.Map<DTOs.Others.ProfileIdWithName>).ToList());
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new CommonException(BaseErrorCode.SEED_DATA_ERROR, StatusCodes.Status422UnprocessableEntity);
            }
        }

        public async Task<IList<string>> GetPotentialProfileIds(string location)
        {
            try
            {
                logger.LogInformation("Starting get page profile ids");
                var response = await client.getPotentialProfileIdsAsync(new PotentialProfilesRequest { Location = location });
                // Call the gRPC server to introspect the token
                return new List<string>(response.Ids);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new CommonException(BaseErrorCode.SEED_DATA_ERROR, StatusCodes.Status422UnprocessableEntity);
            }
        }

        public async Task<IList<DTOs.Others.PreviewFileMetadata>> GetPreviewFileMetadatas()
        {
            try
            {
                logger.LogInformation("Starting get file metadata ids with types");
                var response = await client.getPreviewFileMetadatasAsync(new Empty());
                // Call the gRPC server to introspect the token
                return new List<DTOs.Others.PreviewFileMetadata>(response.PreviewFileMetadatas
                    .Select(mapper.Map<DTOs.Others.PreviewFileMetadata>).ToList());
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new CommonException(BaseErrorCode.SEED_DATA_ERROR, StatusCodes.Status422UnprocessableEntity);
            }
        }

        public async Task<IList<DTOs.Responses.Profile.BaseProfileResponse>> GetProfiles(IList<string> ids)
        {
            try
            {
                logger.LogInformation("Starting get user profile");
                var request = new ProfilesRequest();
                request.Ids.AddRange(ids);
                var response = await client.getProfilesAsync(request);
                return response.Profiles.Select(mapper.Map<DTOs.Responses.Profile.BaseProfileResponse>).ToList();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new CommonException(BaseErrorCode.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);
            }
        }

    }
}
