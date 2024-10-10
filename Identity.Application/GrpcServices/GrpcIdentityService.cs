using Grpc.Core;
using Microsoft.Extensions.Logging;
using InfinityNetServer.Services.Identity.Application.Interfaces;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Application.GrpcServices
{
    public class GrpcIdentityService : IdentityService.IdentityServiceBase
    {

        private readonly ILogger<GrpcIdentityService> _logger;

        private readonly IAuthService _authService;

        public GrpcIdentityService(ILogger<GrpcIdentityService> logger, IAuthService authService) : base()
        {
            _logger = logger;
            _authService = authService;
        }

        public override async Task<IntrospectResponse> introspect(IntrospectRequest request, ServerCallContext context)
        {
            // Log the request
            _logger.LogInformation("Received introspect request for token: {Token}", request.Token);

            // Validate the token using _authService
            var result = await _authService.Introspect(request.Token);

            return new IntrospectResponse
            {
                Valid = result,
            };
        }

    }
}
