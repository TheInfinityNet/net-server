using AutoMapper;
using Grpc.Core;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.Services.Reaction.Application.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

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

        public override async Task<ReactionTypeResponse> getPostReactionByProfileId(ReactionByPostIdAndProfileIdRequest request, ServerCallContext context)
        {
            logger.LogInformation("Handling GRPC call for getPostReactionByProfileId with PostId: {PostId} and ProfileId: {ProfileId}", request.PostId, request.ProfileId);

            var reaction = await postReactionService.GetByPostIdAndProfileId(request.PostId, request.ProfileId);

            return new ReactionTypeResponse
            {
                Type = reaction.Type.ToString()
            };
        }

        public override async Task<ReactionTypeResponse> getCommentReactionByProfileId(ReactionByCommentIdAndProfileIdRequest request, ServerCallContext context)
        {
            logger.LogInformation("Handling GRPC call for getCommentReactionByProfileId with CommentId: {CommentId} and ProfileId: {ProfileId}", request.CommentId, request.ProfileId);

            var reaction = await commentReactionService.GetByCommentIdAndProfileId(request.CommentId, request.ProfileId);

            return new ReactionTypeResponse
            {
                Type = reaction.Type.ToString()
            };
        }

    }
}
