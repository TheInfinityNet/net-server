using InfinityNetServer.Services.Reaction.Domain.Entities;
using InfinityNetServer.Application.DTOs.Results;
using InfinityNetServer.Application.Post.Presentation.DTOs.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Application.Services
{
    public interface ICommentReactionService
    {
        public Task<CommentReaction> CreateAsync(AddCommentReactionRequest request);
        public Task<List<CommandReacionGroupResult>> GetCommandReaction(string lstCommandId);

    }
}