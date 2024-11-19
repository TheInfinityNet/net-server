using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using InfinityNetServer.Services.Post.Application.Services;
using InfinityNetServer.Services.Post.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Post.Application.GrpcServices
{
    public class GrpcPostService(
        ILogger<GrpcPostService> logger,
        IMapper mapper,
        IPostService postService) : PostService.PostServiceBase
    {

        public override async Task<PostIdsResponse> getPostIds(Empty request, ServerCallContext context)
        {
            logger.LogInformation("Received getPostIds request");
            var response = new PostIdsResponse();
            var postIds = await postService.GetAllPresentationIds();
            response.Ids.AddRange(postIds);

            return await Task.FromResult(response);
        }

        public override async Task<PreviewFileMetadatasResponse> getPreviewFileMetadatas(PreviewFileMetadatasRequest request, ServerCallContext context)
        {
            logger.LogInformation("Received getFileMetadataIdsWithTypes request");
            var response = new PreviewFileMetadatasResponse();
            var fileMetadataIdsWithTypes = await postService.GetByType(request.Type.ToString());
            response.PreviewFileMetadatas.AddRange(fileMetadataIdsWithTypes.Select(p => new PreviewFileMetadata
            {
                Id = p.Id.ToString(),
                OwnerId = p.OwnerId.ToString(),
                FileMetadataId = p.FileMetadataId.ToString(),
                Type = p.Type.ToString()
            }));

            return await Task.FromResult(response);
        }

        public override async Task<PreviewPostResponse> getPreviewPost(PostRequest request, ServerCallContext context)
        {
            logger.LogInformation("Received previewPost request");
            var post = await postService.GetById(request.Id);
            post.FileMetadataId ??= Guid.Empty;
            return mapper.Map<PreviewPostResponse>(post);
        }

        public override async Task<ProfileIdsResponse> whoCantSee(PostRequest request, ServerCallContext context)
        {
            logger.LogInformation("Received whoCantSee request");
            var response = new ProfileIdsResponse();
            var profileIds = await postService.WhoCantSee(request.Id);
            response.Ids.AddRange(profileIds);
            return await Task.FromResult(response);
        }

    }
}
