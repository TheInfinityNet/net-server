using Grpc.Core;
using Microsoft.Extensions.Logging;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using System.Linq;
using InfinityNetServer.Services.Comment.Domain.Repositories;

namespace InfinityNetServer.Services.Comment.Application.GrpcServices
{
    public class GrpcCommentService : CommentService.CommentServiceBase
    {

        private readonly ILogger<GrpcCommentService> _logger;

        private readonly ICommentRepository _commentRepository;

        public GrpcCommentService(ILogger<GrpcCommentService> logger, ICommentRepository commentRepository)
        {
            _logger = logger;
            _commentRepository = commentRepository;
        }

        public override async Task<GetCommentIdsResponse> getCommentIds(Empty request, ServerCallContext context)
        {
            _logger.LogInformation("Received getAccountIds request");
            var response = new GetCommentIdsResponse();
            var comments = await _commentRepository.GetAllAsync();
            response.CommentIds.AddRange(comments.Select(p => p.Id.ToString()).ToList());

            return await Task.FromResult(response);
        }

    }
}
