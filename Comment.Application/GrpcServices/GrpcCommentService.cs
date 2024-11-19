using Grpc.Core;
using Microsoft.Extensions.Logging;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using System.Linq;
using InfinityNetServer.Services.Comment.Domain.Repositories;
using System;
using AutoMapper;

namespace InfinityNetServer.Services.Comment.Application.GrpcServices
{
    public class GrpcCommentService(
        ILogger<GrpcCommentService> logger,
        IMapper mapper,
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

    }
}
