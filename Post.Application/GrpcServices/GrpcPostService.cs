using Grpc.Core;
using Microsoft.Extensions.Logging;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using System.Linq;
using InfinityNetServer.Services.Post.Domain.Repositories;

namespace InfinityNetServer.Services.Post.Application.GrpcServices
{
    public class GrpcPostService(ILogger<GrpcPostService> logger, IPostRepository postRepository) : PostService.PostServiceBase
    {

        public override async Task<GetPostIdsResponse> getPostIds(Empty request, ServerCallContext context)
        {
            logger.LogInformation("Received getAccountIds request");
            var response = new GetPostIdsResponse();
            var posts = await postRepository.GetAllAsync();
            response.Ids.AddRange(posts.Select(p => p.Id.ToString()).ToList());

            return await Task.FromResult(response);
        }

    }
}
