using Grpc.Core;
using Microsoft.Extensions.Logging;
using InfinityNetServer.Services.Identity.Application.Interfaces;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using InfinityNetServer.Services.Identity.Domain.Repositories;

namespace InfinityNetServer.Services.Identity.Application.GrpcServices
{
    public class GrpcIdentityService : IdentityService.IdentityServiceBase
    {

        private readonly ILogger<GrpcIdentityService> _logger;

        private readonly IAuthService _authService;

        private readonly IAccountRepository accountRepository;

        public GrpcIdentityService(ILogger<GrpcIdentityService> logger, IAuthService authService, IAccountRepository accountRepository)
        {
            _logger = logger;
            _authService = authService;
            this.accountRepository = accountRepository;
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

            return await Task.FromResult(response);
        }

        public override async Task<GetAccountIdsResponse> getAccountIds(Empty request, ServerCallContext context)
        {
            _logger.LogInformation("Received getAccountIds request");
            var response = new GetAccountIdsResponse();
            response.AccountIds.AddRange(await accountRepository.GetAllAccountIdsAsync());

            return await Task.FromResult(response);
        }

    }
}
