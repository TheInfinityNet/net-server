using Google.Protobuf.WellKnownTypes;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.BuildingBlocks.Application.GrpcClients
{
    public class IdentityClient
    {

        private readonly IdentityService.IdentityServiceClient _client;

        private readonly ILogger<IdentityClient> _logger;

        public IdentityClient(IdentityService.IdentityServiceClient client, ILogger<IdentityClient> logger)
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

                // Call the gRPC server to introspect the token
                var response = await _client.getAccountIdsAsync(new Empty());
                return new List<string>(response.AccountIds);
            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex.Message);
                return [];
            }
        }

    }
}
