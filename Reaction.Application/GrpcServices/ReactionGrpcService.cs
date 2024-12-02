using AutoMapper;
using Grpc.Core;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using InfinityNetServer.Services.Reaction.Application.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReactionType = InfinityNetServer.BuildingBlocks.Domain.Enums.ReactionType;

namespace InfinityNetServer.Services.Reaction.Application.GrpcServices
{
    public class ReactionGrpcService(
        IMapper mapper,
        ILogger<ReactionGrpcService> logger,
        IPostReactionService postReactionService,
        ICommentReactionService commentReactionService) : ReactionService.ReactionServiceBase
    {

        public override async Task<ReactionCountsResponse> getPostReactionsCount(ReactionCountsRequest request, ServerCallContext context)
        {
            logger.LogInformation("Handling GRPC call for getPostReactionsCounts");

            IList<(string postId, IDictionary<ReactionType, int> countDetails)> 
                reactionsCounts = await postReactionService.CountByPostIdAsync(request.OwnerIds);

            var response = new ReactionCountsResponse();
            response.ReactionCounts.AddRange(reactionsCounts.Select(q => new ReactionCountWithOwnerId
            {
                OwnerId = q.postId,
                CountDetails = { q.countDetails.Select(p => new ReactionCount
                {
                    Type = p.Key.ToString(),
                    Count = p.Value
                })}
            }));

            return response;
        }

        public override async Task<ReactionCountsResponse> getCommentReactionsCount(ReactionCountsRequest request, ServerCallContext context)
        {
            logger.LogInformation("Handling GRPC call for getCommentReactionsCounts");

            IList<(string commentId, IDictionary<ReactionType, int> countDetails)>
                reactionsCounts = await commentReactionService.CountByCommentIdAsync(request.OwnerIds);

            var response = new ReactionCountsResponse();
            response.ReactionCounts.AddRange(reactionsCounts.Select(q => new ReactionCountWithOwnerId
            {
                OwnerId = q.commentId,
                CountDetails = { q.countDetails.Select(p => new ReactionCount
                {
                    Type = p.Key.ToString(),
                    Count = p.Value
                })}
            }));

            return response;
        }

        public override async Task<PreviewReactionsResponse> getPostReactionsByProfileIds(ReactionsByProfileIdsRequest request, ServerCallContext context)
        {
            logger.LogInformation("Handling GRPC call for getPostReactionByProfileId");
            IList<(string postId, string profileId)> input = [];
            foreach (var item in request.OwnerIdsAndProfileIds)
                input.Add((item.OwnerId, item.ProfileId));

            var reaction = await postReactionService.GetAllByPostIdsAndProfileIds(input);

            var response = new PreviewReactionsResponse();
            response.PreviewReactions.AddRange(reaction.Select(mapper.Map<PreviewReaction>));
            return response;
        }

        public override async Task<PreviewReactionsResponse> getCommentReactionsByProfileIds(ReactionsByProfileIdsRequest request, ServerCallContext context)
        {
            logger.LogInformation("Handling GRPC call for getCommentReactionByProfileId");
            IList<(string commentId, string profileId)> input = [];
            foreach (var item in request.OwnerIdsAndProfileIds)
                input.Add((item.OwnerId, item.ProfileId));

            var reaction = await commentReactionService.GetAllByCommentIdsAndProfileIds(input);

            var response = new PreviewReactionsResponse();
            response.PreviewReactions.AddRange(reaction.Select(mapper.Map<PreviewReaction>));
            return response;
        }

    }
}
