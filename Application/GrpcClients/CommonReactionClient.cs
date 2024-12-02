using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
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

        public async Task<IList<(string postId, IDictionary<string, int> countDetails)>> GetPostReactionsCount(IList<string> postIds)
        {
            try
            {
                logger.LogInformation("Starting get post reactions count");
                var request = new ReactionCountsRequest();
                request.OwnerIds.AddRange(postIds);

                var result = await client.getPostReactionsCountAsync(request);

                return result.ReactionCounts.Select(q => {
                    IDictionary<string, int> countDetails = q.CountDetails.ToDictionary(p => p.Type, p => p.Count);
                    return (q.OwnerId, countDetails);
                }).ToList();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                //throw new BaseException(BaseError.REACTION_NOT_FOUND, StatusCodes.Status422UnprocessableEntity);
                return [];
            }
        }

        public async Task<IList<(string commentId, IDictionary<string, int> countDetails)>> GetCommentReactionsCount(IList<string> commentIds)
        {
            try
            {
                logger.LogInformation("Starting get comment reactions count");
                var request = new ReactionCountsRequest();
                request.OwnerIds.AddRange(commentIds);

                var result = await client.getCommentReactionsCountAsync(request);

                return result.ReactionCounts.Select(q => {
                    IDictionary<string, int> countDetails = q.CountDetails.ToDictionary(p => p.Type, p => p.Count);
                    return (q.OwnerId, countDetails);
                }).ToList();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                //throw new BaseException(BaseError.REACTION_NOT_FOUND, StatusCodes.Status422UnprocessableEntity);
                return [];
            }
        }

        public async Task<IList<DTOs.Others.PreviewReaction>> GetPostReactionsByProfileIds(IList<(string postId, string profileId)> postIdsAndProfileIds)
        {
            try
            {
                logger.LogInformation("Starting get post reactions by profile ids");
                var request = new ReactionsByProfileIdsRequest();
                request.OwnerIdsAndProfileIds.AddRange(postIdsAndProfileIds
                    .Select(q => new ReactionByOwnerIdAndProfileId { OwnerId = q.postId, ProfileId = q.profileId }));

                var reactionTypes = await client.getPostReactionsByProfileIdsAsync(request);
                // Call the gRPC server to introspect the token
                return reactionTypes.PreviewReactions.Select(mapper.Map<DTOs.Others.PreviewReaction>).ToList();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                //throw new BaseException(BaseError.REACTION_NOT_FOUND, StatusCodes.Status422UnprocessableEntity);
                return [];
            }
        }

        public async Task<IList<DTOs.Others.PreviewReaction>> GetCommentReactionByProfileId(IList<(string commentId, string profileId)> commentIdsAndProfileIds)
        {
            try
            {
                logger.LogInformation("Starting get comment reactions by profile ids");
                var request = new ReactionsByProfileIdsRequest();
                request.OwnerIdsAndProfileIds.AddRange(commentIdsAndProfileIds
                   .Select(q => new ReactionByOwnerIdAndProfileId { OwnerId = q.commentId, ProfileId = q.profileId }));

                var reactionTypes = await client.getCommentReactionsByProfileIdsAsync(request);
                // Call the gRPC server to introspect the token
                return reactionTypes.PreviewReactions.Select(mapper.Map<DTOs.Others.PreviewReaction>).ToList();
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
