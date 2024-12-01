using AutoMapper;
using InfinityNetServer.Services.Reaction.Domain.Entities;
using InfinityNetServer.Services.Reaction.Domain.Repositories;
using InfinityNetServer.Services.Reaction.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfinityNetServer.Services.Reaction.Application.Services;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.Services.Reaction.Application.DTOs.Results;
using InfinityNetServer.Services.Reaction.Application.DTOs.Requests;

namespace InfinityNetServer.Services.Reaction.Presentation.Services
{
    public class PostReactionService(IPostReactionRepository repository, IMapper mapper, ReactionDbContext context) : IPostReactionService
    {

        public async Task<int> CountByPostIdAndType(string postId, ReactionType type)
            => await repository.CountByPostIdAndType(Guid.Parse(postId), type);

        public async Task<PostReaction> Create(AddPostReactionRequest request)
        {
            var model = mapper.Map<PostReaction>(request);
            return await repository.CreateAsync(model);
        }

        public async Task<IList<PostReaction>> GetAllByPostIdsAndProfileIds(IList<(string postId, string profileId)> postIdsAndProfileIds)
            => await repository.GetAllByPostIdsAndProfileIdsAsync(postIdsAndProfileIds.Select(q => (Guid.Parse(q.postId), Guid.Parse(q.profileId))).ToList());

        public async Task<List<PostReactionGroupResult>> GetPostReactions(string lstPostId)
        {
            var lstId = lstPostId.Split(',').Select(q => Guid.TryParse(q, out Guid postId) ? postId : Guid.Empty)
                .Where(q => q != Guid.Empty).ToList();
            if (lstId.Count == 0) return [];
            var request = await context.PostReactions
                .Where(q => lstId.Any(p => p.Equals(q.PostId)))
                .GroupBy(q => new { q.PostId, q.Type })
                .Select(q => new PostReactionGroupResult()
                {
                    Count = q.Count(),
                    PostId = q.Key.PostId,
                    Type = q.Key.Type,
                }).ToListAsync();
            return request;
        }
    }
}