using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static InfinityNetServer.BuildingBlocks.Application.Protos.CommentService;

namespace InfinityNetServer.BuildingBlocks.Application.GrpcClients
{
    public class CommonCommentClient(CommentServiceClient client, ILogger<CommonCommentClient> logger, IMapper mapper)
    {

        public async Task<List<string>> GetCommentIds()
        {
            try
            {
                logger.LogInformation("Starting get account ids");
                var response = await client.getCommentIdsAsync(new Empty());
                // Call the gRPC server to introspect the token
                return new List<string>(response.Ids);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new CommonException(BaseErrorCode.SEED_DATA_ERROR, StatusCodes.Status422UnprocessableEntity);
            }
        }

        public async Task<List<DTOs.Others.FileMetadataIdWithOwnerId>> GetPreviewFileMetadatas()
        {
            try
            {
                logger.LogInformation("Starting get file metadata ids with types");
                var response = await client.getPreviewFileMetadatasAsync(new Empty());
                // Call the gRPC server to introspect the token
                return new List<DTOs.Others.FileMetadataIdWithOwnerId>(response.PreviewFileMetadatas
                    .Select(mapper.Map<DTOs.Others.FileMetadataIdWithOwnerId>).ToList());
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new CommonException(BaseErrorCode.SEED_DATA_ERROR, StatusCodes.Status422UnprocessableEntity);
            }
        }

        public async Task<DTOs.Responses.Comment.PreviewCommentResponse> GetPreviewComment(string id)
        {
            try
            {
                logger.LogInformation("Starting get preview comment");
                var response = await client.getPreviewCommentAsync(new PreviewCommentRequest { Id = id });
                // Call the gRPC server to introspect the token
                return mapper.Map<DTOs.Responses.Comment.PreviewCommentResponse>(response);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new CommonException(BaseErrorCode.COMMENT_NOT_FOUND, StatusCodes.Status404NotFound);
            }
        }

        public async Task<int> GetCommentCount(string postId)
        {
            try
            {
                logger.LogInformation("Starting get preview comment");
                var response = await client.getCommentCountAsync(new CommentCountRequest { PostId = postId });
                // Call the gRPC server to introspect the token
                return response.Count;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new CommonException(BaseErrorCode.COMMENT_NOT_FOUND, StatusCodes.Status404NotFound);
            }
        }

    }
}
