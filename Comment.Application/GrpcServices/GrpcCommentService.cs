using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using InfinityNetServer.Services.Comment.Application.Services;
using InfinityNetServer.Services.Comment.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Comment.Application.GrpcServices
{
    public class GrpcCommentService(
        ILogger<GrpcCommentService> logger,
        IMapper mapper,
        ICommentService commentService,
        ICommentRepository commentRepository) : CommentService.CommentServiceBase
    {

        public override async Task<CommentIdsResponse> getCommentIds(Empty request, ServerCallContext context)
        {
            logger.LogInformation("Received getAccountIds request");
            var response = new CommentIdsResponse();
            var comments = await commentRepository.GetAllAsync();
            response.Ids.AddRange(comments.Select(p => p.Id.ToString()).ToList());

            return await Task.FromResult(response);
        }

        public override async Task<PreviewFileMetadatasResponse> getPreviewFileMetadatas(Empty request, ServerCallContext context)
        {
            logger.LogInformation("Received getFileMetadataIdsWithTypes request");
            var response = new PreviewFileMetadatasResponse();
            var comments = await commentRepository.GetAllMediaCommentAsync();
            response.PreviewFileMetadatas.AddRange(comments.Select(p => new PreviewFileMetadata
            {
                Id = p.Id.ToString(),
                OwnerId = p.ProfileId.ToString(),
                FileMetadataId = p.FileMetadataId.ToString()
            }));

            return await Task.FromResult(response);
        }

        public override async Task<PreviewCommentResponse> getPreviewComment(PreviewCommentRequest request, ServerCallContext context)
        {
            logger.LogInformation("Received getPreviewComment request");
            Domain.Entities.Comment comment = await commentRepository.GetByIdAsync(Guid.Parse(request.Id));
            comment.FileMetadataId ??= Guid.Empty;
            return mapper.Map<PreviewCommentResponse>(comment);
        }

        public override async Task<CommentCountResponse> getCommentCount(CommentByPostIdRequest request, ServerCallContext context)
        {
            var getPostIdRequest = new CommentByPostIdRequest
            {
                PostId = request.PostId
            };
            logger.LogInformation("GetCommentCount");
            var source = await commentService.GetCommentCountAsync(getPostIdRequest.PostId);
            var response = new CommentCountResponse
            {
                Count = source.CommentCount
            };
            return response;
        }

        public override async Task<CommentPreviewResponse> getCommentPreview(CommentByPostIdRequest request, ServerCallContext context)
        {
            var getPostIdRequest = new CommentByPostIdRequest
            {
                PostId = request.PostId
            };
            logger.LogInformation("GetCommentPreview");
            var source = await commentService.GetTopCommentWithMostRepliesAsync(getPostIdRequest.PostId);
            var response = mapper.Map<CommentPreviewResponse>(source);
            logger.LogInformation("GetCommentPreview");
            return response;
        }

        public override async Task<CommentIdsResponse> getCommentIdsByPostId(CommentByPostIdRequest request, ServerCallContext context)
        {
            logger.LogInformation("Received getCommentIds by postId request");
            var response = new CommentIdsResponse();
            var comments = await commentRepository.GetAllByPostIdAsync(Guid.Parse(request.PostId));
            response.Ids.AddRange(comments.Select(p => p.Id.ToString()).ToList());

            return await Task.FromResult(response);
        }
    }
}
