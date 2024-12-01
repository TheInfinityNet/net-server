using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.Services.Reaction.Application.DTOs.Requests;
using InfinityNetServer.Services.Reaction.Application.DTOs.Results;
using InfinityNetServer.Services.Reaction.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Reaction.Application.Services
{
    public interface IPostReactionService
    {

        public Task<PostReaction> Create(AddPostReactionRequest request);

        public Task<int> CountByPostIdAndType(string postId, ReactionType type);

        public Task<IList<PostReaction>> GetAllByPostIdsAndProfileIds(IList<(string postId, string profileId)> postIdsAndProfileIds);

        public Task<List<PostReactionGroupResult>> GetPostReactions(string lstPostId);// Id cách nhau bởi ,

    }
}