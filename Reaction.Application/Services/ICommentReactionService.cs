using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.Services.Reaction.Application.DTOs.Requests;
using InfinityNetServer.Services.Reaction.Application.DTOs.Results;
using InfinityNetServer.Services.Reaction.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Reaction.Application.Services
{
    public interface ICommentReactionService
    {
        public Task<CommentReaction> CreateAsync(AddCommentReactionRequest request);

        public Task<int> CountByCommentIdAndType(string commentId, ReactionType type);

        public Task<CommentReaction> GetByCommentIdAndProfileId(string commentId, string profileId);

        public Task<List<CommandReacionGroupResult>> GetCommandReaction(string lstCommandId);

    }
}