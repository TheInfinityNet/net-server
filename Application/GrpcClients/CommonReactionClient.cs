using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using static InfinityNetServer.BuildingBlocks.Application.Protos.ReactionService;

namespace InfinityNetServer.BuildingBlocks.Application.GrpcClients
{
    public class CommonReactionClient(ReactionServiceClient client, ILogger<CommonReactionClient> logger, IMapper mapper)
    {

        public async Task<int> GetPostReactionsCount(string postId, string type)
        {
            try
            {
                logger.LogInformation("Starting get post reactions count");
                var response = await client.getPostReactionsCountAsync(new ReactionsByPostIdRequest { PostId = postId, Type = type });
                // Call the gRPC server to introspect the token
                return response.Count;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new BaseException(BaseError.REACTION_NOT_FOUND, StatusCodes.Status422UnprocessableEntity);
            }
        }

        public async Task<int> GetCommentReactionsCount(string commentId, string type)
        {
            try
            {
                logger.LogInformation("Starting get comment reactions count");
                var response = await client.getCommentReactionsCountAsync(new ReactionsByCommentIdRequest { CommentId = commentId, Type = type });
                // Call the gRPC server to introspect the token
                return response.Count;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new BaseException(BaseError.REACTION_NOT_FOUND, StatusCodes.Status422UnprocessableEntity);
            }
        }

        public async Task<string> GetPostReactionByProfileId(string postId, string profileId)
        {
            try
            {
                logger.LogInformation("Starting get post reaction by profile id");
                var response = await client.getPostReactionByProfileIdAsync(new ReactionByPostIdAndProfileIdRequest { PostId = postId, ProfileId = profileId });
                // Call the gRPC server to introspect the token
                return response.Type;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new BaseException(BaseError.REACTION_NOT_FOUND, StatusCodes.Status422UnprocessableEntity);
            }
        }

        public async Task<string> GetCommentReactionByProfileId(string commentId, string profileId)
        {
            try
            {
                logger.LogInformation("Starting get comment reaction by profile id");
                var response = await client.getCommentReactionByProfileIdAsync(new ReactionByCommentIdAndProfileIdRequest { CommentId = commentId, ProfileId = profileId });
                // Call the gRPC server to introspect the token
                return response.Type;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new BaseException(BaseError.REACTION_NOT_FOUND, StatusCodes.Status422UnprocessableEntity);
            }
        }

    }
}
