using Grpc.Core;
using Microsoft.Extensions.Logging;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using System.Linq;
using InfinityNetServer.Services.Comment.Domain.Repositories;

namespace InfinityNetServer.Services.Comment.Application.GrpcServices
{
    public class GrpcCommentService(ILogger<GrpcCommentService> logger, ICommentRepository commentRepository) : CommentService.CommentServiceBase
    {

        public override async Task<GetCommentIdsResponse> getCommentIds(Empty request, ServerCallContext context)
        {
            logger.LogInformation("Received getAccountIds request");
            var response = new GetCommentIdsResponse();
            var comments = await commentRepository.GetAllAsync();
            response.Ids.AddRange(comments.Select(p => p.Id.ToString()).ToList());

            return await Task.FromResult(response);
        }

        public override async Task<GetFileMetadataIdsWithOwnerIdsResponse> getFileMetadataIdsWithOwnerIds(Empty request, ServerCallContext context)
        {
            logger.LogInformation("Received getFileMetadataIdsWithTypes request");
            var response = new GetFileMetadataIdsWithOwnerIdsResponse();
            var comments = await commentRepository.GetAllMediaCommentAsync();
            response.FileMetadataIdsWithOwnerIds.AddRange(comments.Select(p => new FileMetadataIdWithOwnerId
            {
                Id = p.Id.ToString(),
                OwnerId = p.ProfileId.ToString(),
                FileMetadataId = p.Id.ToString()
            }));

            return await Task.FromResult(response);
        }

    }
}
