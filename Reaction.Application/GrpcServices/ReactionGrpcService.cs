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

        public override async Task<ReactionCountResponse> getPostReactionsCount(ReactionsByPostIdRequest request, ServerCallContext context)
        {
            logger.LogInformation("Handling GRPC call for getPostReactionsCount with PostId: {PostId}", request.PostId);

            ReactionType reactionType = Enum.Parse<ReactionType>(request.Type);

            var count = await postReactionService.CountByPostIdAndType(request.PostId, reactionType);

            return new ReactionCountResponse
            {
                Count = count
            };
        }

        public override async Task<ReactionCountResponse> getCommentReactionsCount(ReactionsByCommentIdRequest request, ServerCallContext context)
        {
            logger.LogInformation("Handling GRPC call for getCommentReactionsCount with CommentId: {CommentId}", request.CommentId);

            ReactionType reactionType = Enum.Parse<ReactionType>(request.Type);

            var count = await commentReactionService.CountByCommentIdAndType(request.CommentId, reactionType);

            return new ReactionCountResponse
            {
                Count = count
            };
        }

        public override async Task<ReactionTypesResponse> getPostReactionsByProfileIds(ReactionsByPostIdsAndProfileIdsRequest request, ServerCallContext context)
        {
            logger.LogInformation("Handling GRPC call for getPostReactionByProfileId");
            IList<(string postId, string profileId)> input = [];
            foreach (var item in request.PostIdsAndProfileIds)
                input.Add((item.PostId, item.ProfileId));

            var reaction = await postReactionService.GetAllByPostIdsAndProfileIds(input);

            var response = new ReactionTypesResponse();
            response.Types_.AddRange(reaction.Select(p => new BuildingBlocks.Application.Protos.ReactionType
            {
                Type = p.Type.ToString()
            }));
            return response;
        }

        public override async Task<ReactionTypesResponse> getCommentReactionsByProfileIds(ReactionsByCommentIdsAndProfileIdsRequest request, ServerCallContext context)
        {
            logger.LogInformation("Handling GRPC call for getCommentReactionByProfileId");
            IList<(string commentId, string profileId)> input = [];
            foreach (var item in request.CommentIdsAndProfileIds)
                input.Add((item.CommentId, item.ProfileId));

            var reaction = await commentReactionService.GetAllByCommentIdsAndProfileIds(input);

            var response = new ReactionTypesResponse();
            response.Types_.AddRange(reaction.Select(p => new BuildingBlocks.Application.Protos.ReactionType
            {
                Type = p.Type.ToString()
            }));
            return response;
        }

    }
}
