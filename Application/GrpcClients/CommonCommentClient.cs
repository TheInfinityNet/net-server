using AutoMapper;
using Google.Protobuf.WellKnownTypes;
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

        public async Task<IList<string>> GetCommentIds()
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
                throw new BaseException(BaseError.COMMENT_NOT_FOUND, StatusCodes.Status422UnprocessableEntity);
            }
        }

        public async Task<IList<DTOs.Others.PreviewFileMetadata>> GetPreviewFileMetadatas()
        {
            try
            {
                logger.LogInformation("Starting get file metadata ids with types");
                var response = await client.getPreviewFileMetadatasAsync(new Empty());
                // Call the gRPC server to introspect the token
                return new List<DTOs.Others.PreviewFileMetadata>(response.PreviewFileMetadatas
                    .Select(mapper.Map<DTOs.Others.PreviewFileMetadata>).ToList());
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new BaseException(BaseError.COMMENT_NOT_FOUND, StatusCodes.Status422UnprocessableEntity);
            }
        }

        public async Task<IList<string>> GetCommentIdsByPostId(string postId)
        {
            try
            {
                logger.LogInformation("Starting get comment ids by post id");
                var response = await client.getCommentIdsByPostIdAsync(new CommentByPostIdRequest { PostId = postId });
                // Call the gRPC server to introspect the token
                return new List<string>(response.Ids);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new BaseException(BaseError.COMMENT_NOT_FOUND, StatusCodes.Status404NotFound);
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
                throw new BaseException(BaseError.COMMENT_NOT_FOUND, StatusCodes.Status404NotFound);
            }
        }

        public async Task<int> GetCommentCountByPostId(string postId)
        {
            try
            {
                logger.LogInformation("Starting get comment count");
                var response = await client.getCommentCountByPostIdAsync(new CommentByPostIdRequest { PostId = postId });
                // Call the gRPC server to introspect the token
                return response.Count;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Caused by {0}", e.Message);
                //throw new BaseException(BaseError.COMMENT_NOT_FOUND, StatusCodes.Status404NotFound);
                return 0;
            }
        }

        public async Task<IList<DTOs.Responses.Comment.CommentResponse>> GetPopularComments(string postId)
        {
            try
            {
                logger.LogInformation("Starting get popular comments");
                var response = await client.getPopularCommentsAsync(new CommentByPostIdRequest { PostId = postId });
                // Call the gRPC server to introspect the token
                return response.Comments.Select(mapper.Map<DTOs.Responses.Comment.CommentResponse>).ToList();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Caused by {0}", e.Message);
                //throw new BaseException(BaseError.COMMENT_NOT_FOUND, StatusCodes.Status404NotFound);
                return [];
            }
        }

    }
}
