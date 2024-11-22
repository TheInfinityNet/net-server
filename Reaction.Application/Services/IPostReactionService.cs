using InfinityNetServer.Services.Reaction.Domain.Entities;
using InfinityNetServer.Application.DTOs.Results;
using InfinityNetServer.Application.Post.Presentation.DTOs.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Application.Services
{
    public interface IPostReactionService
    {
        public Task<PostReaction> CreatePostReaction(AddPostReactionRequest request);
        public Task<List<PostReactionGroupResult>> GetPostReactions(string lstPostId);// Id cách nhau bởi ,
    }
}