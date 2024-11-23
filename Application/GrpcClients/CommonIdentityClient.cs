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
    public class CommonIdentityClient(IdentityServiceClient client, ILogger<CommonIdentityClient> logger, IMapper mapper)
    {

        public async Task<bool> Introspect(string token)
        {
            try
            {
                logger.LogInformation("Starting token introspection for token: {Token}", token);

                // Call the gRPC server to introspect the token
                var response = await client.introspectAsync(new IntrospectRequest
                {
                    Token = token
                });
                logger.LogInformation(" is valid: " + response.Valid);
                return response.Valid;
            }
            catch (Exception ex)
            {
                logger.LogError(message: ex.Message);
                return false;
            }
        }

        public async Task<string> GetAccountId(string defaultUserProfileId)
        {
            try
            {
                logger.LogInformation("Starting get account id");
                var response = await client.getAccountIdAsync(new GetAccountIdRequest
                {
                    DefaultUserProfileId = defaultUserProfileId
                });
                // Call the gRPC server to introspect the token
                return response.Id;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new BaseException(BaseError.ACCOUNT_NOT_FOUND, StatusCodes.Status422UnprocessableEntity);
            }
        }

        public async Task<IList<string>> GetAccountIds()
        {
            try
            {
                logger.LogInformation("Starting get account ids");
                var response = await client.getAccountIdsAsync(new Empty());
                // Call the gRPC server to introspect the token
                return new List<string>(response.Ids);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new BaseException(BaseError.ACCOUNT_NOT_FOUND, StatusCodes.Status422UnprocessableEntity);
            }
        }

        public async Task<IList<DTOs.Others.AccountWithDefaultProfile>> GetAccountsWithDefaultProfiles()
        {
            try
            {
                logger.LogInformation("Starting get account with default profile ids");
                var response = await client.getAccountsWithDefaultProfilesAsync(new Empty());

                var result = response.AccountsWithDefaultProfiles
                    .Select(_ => mapper.Map<DTOs.Others.AccountWithDefaultProfile>(_)).ToList();

                return result;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new BaseException(BaseError.ACCOUNT_NOT_FOUND, StatusCodes.Status422UnprocessableEntity);
            }
        }

    }
}
