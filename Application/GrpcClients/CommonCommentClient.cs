using Google.Protobuf.WellKnownTypes;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static InfinityNetServer.BuildingBlocks.Application.Protos.CommentService;

namespace InfinityNetServer.BuildingBlocks.Application.GrpcClients
{
    public class CommonCommentClient
    {

        private readonly CommentServiceClient _client;

        private readonly ILogger<CommonCommentClient> _logger;

        public CommonCommentClient(CommentServiceClient client, ILogger<CommonCommentClient> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<List<string>> GetCommentIds()
        {
            try
            {
                _logger.LogInformation("Starting get account ids");
                var response = await _client.getCommentIdsAsync(new Empty());
                // Call the gRPC server to introspect the token
                return new List<string>(response.Ids);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new CommonException(BaseErrorCode.SEED_DATA_ERROR, StatusCodes.Status422UnprocessableEntity);
            }
        }

    }
}
