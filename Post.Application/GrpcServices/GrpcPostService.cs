using Grpc.Core;
using Microsoft.Extensions.Logging;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using System.Linq;
using InfinityNetServer.Services.Post.Domain.Repositories;

namespace InfinityNetServer.Services.Post.Application.GrpcServices
{
    public class GrpcPostService : PostService.PostServiceBase
    {

        private readonly ILogger<GrpcPostService> _logger;

        private readonly IPostRepository _postRepository;

        public GrpcPostService(ILogger<GrpcPostService> logger, IPostRepository postRepository)
        {
            _logger = logger;
            _postRepository = postRepository;
        }

        public override async Task<GetPostIdsResponse> getPostIds(Empty request, ServerCallContext context)
        {
            _logger.LogInformation("Received getAccountIds request");
            var response = new GetPostIdsResponse();
            var posts = await _postRepository.GetAllAsync();
            response.PostIds.AddRange(posts.Select(p => p.Id.ToString()).ToList());

            return await Task.FromResult(response);
        }

    }
}
