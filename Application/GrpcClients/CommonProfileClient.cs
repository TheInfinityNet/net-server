using Google.Protobuf.WellKnownTypes;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static InfinityNetServer.BuildingBlocks.Application.Protos.ProfileService;

namespace InfinityNetServer.BuildingBlocks.Application.GrpcClients
{
    public class CommonProfileClient(ProfileServiceClient client, ILogger<CommonProfileClient> logger)
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

    }
}
