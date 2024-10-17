using Google.Protobuf.WellKnownTypes;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.BuildingBlocks.Application.GrpcClients
{
    public class CommonProfileClient
    {

        private readonly ProfileService.ProfileServiceClient _client;

        private readonly ILogger<CommonProfileClient> _logger;

        public CommonProfileClient(ProfileService.ProfileServiceClient client, ILogger<CommonProfileClient> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<List<string>> GetProfileIds()
        {
            try
            {
                _logger.LogInformation("Starting get account ids");
                var response = await _client.getProfileIdsAsync(new Empty());
                // Call the gRPC server to introspect the token
                return new List<string>(response.ProfileIds);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new CommonException(BaseErrorCode.SEED_DATA_ERROR, StatusCodes.Status422UnprocessableEntity);
            }
        }

    }
}
