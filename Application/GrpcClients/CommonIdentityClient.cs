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
    public class CommonIdentityClient
    {

        private readonly IdentityService.IdentityServiceClient _client;

        private readonly ILogger<CommonIdentityClient> _logger;

        public CommonIdentityClient(IdentityService.IdentityServiceClient client, ILogger<CommonIdentityClient> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<bool> Introspect(string token)
        {
            try
            {
                _logger.LogInformation("Starting token introspection for token: {Token}", token);

                // Call the gRPC server to introspect the token
                var response = await _client.introspectAsync(new IntrospectRequest
                {
                    Token = token
                });
                return response.Valid;
            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex.Message);
                return false;
            }
        }

        public async Task<List<string>> GetAccountIds()
        {
            try
            {
                _logger.LogInformation("Starting get account ids");
                var response = await _client.getAccountIdsAsync(new Empty());
                // Call the gRPC server to introspect the token
                return new List<string>(response.AccountIds);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new CommonException(BaseErrorCode.SEED_DATA_ERROR, StatusCodes.Status422UnprocessableEntity);
            }
        }

    }
}
