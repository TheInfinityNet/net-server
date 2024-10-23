using Grpc.Core;
using Microsoft.Extensions.Logging;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using InfinityNetServer.Services.Identity.Domain.Repositories;
using System.Linq;
using InfinityNetServer.Services.Identity.Application.Services;
using System;

namespace InfinityNetServer.Services.Identity.Application.GrpcServices
{
    public class GrpcIdentityService : IdentityService.IdentityServiceBase
    {

        private readonly ILogger<GrpcIdentityService> _logger;

        private readonly IAuthService _authService;

        private readonly IAccountRepository _accountRepository;

        public GrpcIdentityService(ILogger<GrpcIdentityService> logger, IAuthService authService, IAccountRepository accountRepository)
        {
            _logger = logger;
            _authService = authService;
            _accountRepository = accountRepository;
        }

        public override async Task<IntrospectResponse> introspect(IntrospectRequest request, ServerCallContext context)
        {
            // Log the request
            _logger.LogInformation("Received introspect request for token: {Token}", request.Token);

            // Validate the token using _authService
            var response = new IntrospectResponse
            {
                Valid = await _authService.Introspect(request.Token),
            };

            _logger.LogInformation("Token is valid: {Valid}", response.Valid);

            return await Task.FromResult(response);
        }

        public override async Task<GetAccountIdResponse> getAccountId(GetAccountIdRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Received getAccountId request");
            var response = new GetAccountIdResponse();
            var account = await _accountRepository.GetByDefaultUserProfileIdAsync(Guid.Parse(request.DefaultUserProfileId));

            response.Id = account.Id.ToString();
            return response;
        }

        public override async Task<GetAccountIdsResponse> getAccountIds(Empty request, ServerCallContext context)
        {
            _logger.LogInformation("Received getAccountIds request");
            var response = new GetAccountIdsResponse();
            var accounts = await _accountRepository.GetAllAsync();

            // Map accounts to AccountWithDefaultProfile objects
            response.Ids.AddRange(accounts.Select(a => a.Id.ToString()).ToList());

            return response;
        }

        public override async Task<GetAccountsWithDefaultProfilesResponse> getAccountsWithDefaultProfiles(Empty request, ServerCallContext context)
        {
            _logger.LogInformation("Received getAccountWithDefaultProfileIds request");
            var response = new GetAccountsWithDefaultProfilesResponse();
            var accounts = await _accountRepository.GetAllAsync();

            // Map accounts to AccountWithDefaultProfile objects
            response.AccountsWithDefaultProfiles.AddRange(
                accounts.Select(a => new AccountWithDefaultProfile
                {
                    Id = a.Id.ToString(),
                    DefaultUserProfileId = a.DefaultUserProfileId.ToString()
                }).ToList()
            );

            return response;
        }

    }
}
