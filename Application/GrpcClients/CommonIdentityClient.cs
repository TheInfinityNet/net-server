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
using static InfinityNetServer.BuildingBlocks.Application.Protos.IdentityService;

namespace InfinityNetServer.BuildingBlocks.Application.GrpcClients
{
    public class CommonIdentityClient
    {

        private readonly IdentityServiceClient _client;

        private readonly ILogger<CommonIdentityClient> _logger;

        private readonly IMapper _mapper;

        public CommonIdentityClient(IdentityServiceClient client, ILogger<CommonIdentityClient> logger, IMapper mapper)
        {
            _client = client;
            _logger = logger;
            _mapper = mapper;
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
                _logger.LogInformation(" is valid: " + response.Valid);
                return response.Valid;
            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex.Message);
                return false;
            }
        }

        public async Task<string> GetAccountId(string defaultUserProfileId)
        {
            try
            {
                _logger.LogInformation("Starting get account id");
                var response = await _client.getAccountIdAsync(new GetAccountIdRequest
                {
                    DefaultUserProfileId = defaultUserProfileId
                });
                // Call the gRPC server to introspect the token
                return response.Id;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new CommonException(BaseErrorCode.SEED_DATA_ERROR, StatusCodes.Status422UnprocessableEntity);
            }
        }

        public async Task<List<string>> GetAccountIds()
        {
            try
            {
                _logger.LogInformation("Starting get account ids");
                var response = await _client.getAccountIdsAsync(new Empty());
                // Call the gRPC server to introspect the token
                return new List<string>(response.Ids);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new CommonException(BaseErrorCode.SEED_DATA_ERROR, StatusCodes.Status422UnprocessableEntity);
            }
        }

        public async Task<List<DTOs.Others.AccountWithDefaultProfile>> GetAccountsWithDefaultProfiles()
        {
            try
            {
                _logger.LogInformation("Starting get account with default profile ids");
                var response = await _client.getAccountsWithDefaultProfilesAsync(new Empty());

                var result = response.AccountsWithDefaultProfiles
                    .Select(_ => _mapper.Map<DTOs.Others.AccountWithDefaultProfile>(_)).ToList();

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new CommonException(BaseErrorCode.SEED_DATA_ERROR, StatusCodes.Status422UnprocessableEntity);
            }
        }

    }
}
