using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using InfinityNetServer.Services.Comment.Application.Services;
using InfinityNetServer.Services.Comment.Domain.Enums;
using InfinityNetServer.Services.Comment.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

        public override async Task<PreviewFileMetadatasResponse> getPreviewFileMetadatas(PreviewFileMetadatasRequest request, ServerCallContext context)
        {
            logger.LogInformation("Received getFileMetadataIdsWithTypes request");
            var response = new PreviewFileMetadatasResponse();
            var comments = await commentRepository.GetAllByType(System.Enum.Parse<CommentType>(request.Type));
            response.PreviewFileMetadatas.AddRange(comments.Select(p => new PreviewFileMetadata
            {
                Id = p.Id.ToString(),
                OwnerId = p.ProfileId.ToString(),
                FileMetadataId = p.FileMetadataId.ToString(),
                Type = p.Type.ToString()
            }));

            return await Task.FromResult(response);
        }

        public override async Task<CommentIdsResponse> getCommentIdsByPostId(CommentByPostIdRequest request, ServerCallContext context)
        {
            logger.LogInformation("Received getCommentIds by postId request");
            var response = new CommentIdsResponse();
            var comments = await commentRepository.GetAllByPostIdAsync(Guid.Parse(request.PostId));
            response.Ids.AddRange(comments.Select(p => p.Id.ToString()).ToList());

            return await Task.FromResult(response);
        }

        public override async Task<PreviewCommentResponse> getPreviewComment(PreviewCommentRequest request, ServerCallContext context)
        {
            logger.LogInformation("Received getPreviewComment request");
            Domain.Entities.Comment comment = await commentRepository.GetByIdAsync(Guid.Parse(request.Id));
            comment.FileMetadataId ??= Guid.Empty;
            return mapper.Map<PreviewCommentResponse>(comment);
        }

        public override async Task<CommentCountResponse> getCommentCountByPostId(CommentByPostIdRequest request, ServerCallContext context)
        {
            logger.LogInformation("GetCommentCount");
            var source = await commentService.CountByPostId(request.PostId);
            var response = new CommentCountResponse
            {
                Count = source
            };
            return response;
        }

        public override async Task<PopularCommentsResponse> getPopularComments(CommentByPostIdRequest request, ServerCallContext context)
        {
            logger.LogInformation("Get Popular Comments");
            var source = await commentService.GetPopularComments(request.PostId);
            IList<int> counts = await commentService.CountByParentIds(source.Select(p => p.Id.ToString()).ToList());
            var repliesCountDict = source.ToDictionary(p => p.Id, p => counts[source.IndexOf(p)]);

            var comments = source.Select(comment => {
                comment.FileMetadataId ??= Guid.Empty;
                var dto = mapper.Map<CommentResponse>(comment);
                if (repliesCountDict.TryGetValue(comment.Id, out var count)) dto.ReplyCount = count;
                return dto;
            }).ToList();

            var response = new PopularCommentsResponse();
            response.Comments.AddRange(comments);
            return response;
        }
    }
}
