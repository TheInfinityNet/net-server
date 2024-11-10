using Grpc.Core;
using Microsoft.Extensions.Logging;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using System.Linq;
using InfinityNetServer.Services.Post.Domain.Repositories;
using InfinityNetServer.Services.Post.Application.Services;

namespace InfinityNetServer.Services.Post.Application.GrpcServices
{
    public class GrpcPostService(ILogger<GrpcPostService> logger, IPostRepository postRepository, IPostService postService) : PostService.PostServiceBase
    {

        public override async Task<GetPostIdsResponse> getPostIds(Empty request, ServerCallContext context)
        {
            logger.LogInformation("Received getAccountIds request");
            var response = new GetPostIdsResponse();
            var posts = await postRepository.GetAllAsync();
            response.Ids.AddRange(posts.Select(p => p.Id.ToString()).ToList());

            return await Task.FromResult(response);
        }

        public override async Task<GetFileMetadataIdsWithTypesResponse> getFileMetadataIdsWithTypes(GetFileMetadataIdsWithTypesRequest request, ServerCallContext context)
        {
            logger.LogInformation("Received getFileMetadataIdsWithTypes request");
            var response = new GetFileMetadataIdsWithTypesResponse();
            var fileMetadataIdsWithTypes = await postService.GetByType(request.Type.ToString());
            response.FileMetadataIdsWithTypes.AddRange(fileMetadataIdsWithTypes.Select(p => new FileMetadataIdWithType
            {
                Id = p.Id.ToString(),
                OwnerId = p.OwnerId.ToString(),
                FileMetadataId = p.FileMetadataId.ToString(),
                Type = p.Type.ToString()
            }));

            return await Task.FromResult(response);
        }

        public override async Task<GetFileMetadataIdOfPostResponse> getFileMetadataIdOfPost(GetFileMetadataIdOfPostRequest request, ServerCallContext context)
        {
            logger.LogInformation("Received getFileMetadataIdOfPost request");
            var response = new GetFileMetadataIdOfPostResponse();
            var post = await postService.GetById(request.Id);
            response.FileMetadataId = post != null ? post.FileMetadataId.ToString() : string.Empty;
            return await Task.FromResult(response);
        }

    }
}
