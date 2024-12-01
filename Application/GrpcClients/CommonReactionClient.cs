using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IList<string>> GetPostReactionsByProfileIds(IList<(string postId, string profileId)> postIdsAndProfileIds)
        {
            try
            {
                logger.LogInformation("Starting get post reactions by profile ids");
                var request = new ReactionsByPostIdsAndProfileIdsRequest();
                request.PostIdsAndProfileIds.AddRange(postIdsAndProfileIds
                    .Select(q => new ReactionByPostIdAndProfileId { PostId = q.postId, ProfileId = q.profileId }));

                var reactionTypes = await client.getPostReactionsByProfileIdsAsync(request);
                // Call the gRPC server to introspect the token
                return reactionTypes.Types_.Select(q => q.Type).ToList();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                //throw new BaseException(BaseError.REACTION_NOT_FOUND, StatusCodes.Status422UnprocessableEntity);
                return [];
            }
        }

        public async Task<IList<string>> GetCommentReactionByProfileId(IList<(string commentId, string profileId)> commentIdsAndProfileIds)
        {
            try
            {
                logger.LogInformation("Starting get comment reactions by profile ids");
                var request = new ReactionsByCommentIdsAndProfileIdsRequest();
                request.CommentIdsAndProfileIds.AddRange(commentIdsAndProfileIds
                   .Select(q => new ReactionByCommentIdAndProfileId { CommentId = q.commentId, ProfileId = q.profileId }));

                var reactionTypes = await client.getCommentReactionsByProfileIdsAsync(request);
                // Call the gRPC server to introspect the token
                return reactionTypes.Types_.Select(q => q.Type).ToList();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                //throw new BaseException(BaseError.REACTION_NOT_FOUND, StatusCodes.Status422UnprocessableEntity);
                return [];
            }
        }

    }
}
