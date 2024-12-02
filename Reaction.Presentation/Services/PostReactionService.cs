using AutoMapper;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Domain.Specifications;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.Services.Reaction.Application.DTOs.Requests;
using InfinityNetServer.Services.Reaction.Application.DTOs.Results;
using InfinityNetServer.Services.Reaction.Application.Services;
using InfinityNetServer.Services.Reaction.Domain.Entities;
using InfinityNetServer.Services.Reaction.Domain.Repositories;
using InfinityNetServer.Services.Reaction.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Reaction.Presentation.Services
{
    public class PostReactionService(
        ILogger<PostReactionService> logger,
        IPostReactionRepository repository, 
        IMapper mapper, ReactionDbContext context) : IPostReactionService
    {

        public async Task<IList<(string postId, IDictionary<ReactionType, int> countDetails)>> CountByPostIdAsync(IList<string> postIds)
        {
            var reactionCounts = await repository.CountByPostIdAsync(postIds.Select(Guid.Parse).ToList());
            return reactionCounts.Select(q => (q.postId.ToString(), q.countDetails)).ToList();
        }

        public async Task<IList<PostReaction>> GetAllByPostIdsAndProfileIds(IList<(string postId, string profileId)> postIdsAndProfileIds)
            => await repository.GetAllByPostIdsAndProfileIdsAsync(postIdsAndProfileIds.Select(q => (Guid.Parse(q.postId), Guid.Parse(q.profileId))).ToList());

        public async Task<CursorPagedResult<PostReaction>> GetByPostId
            (string postId, string cursor, int pageSize, ReactionType type)
        {
            var specification = new SpecificationWithCursor<PostReaction>
            {
                Criteria = reaction => reaction.PostId == Guid.Parse(postId) && reaction.Type.Equals(type) && !reaction.IsDeleted,

                OrderFields = [
                        new OrderField<PostReaction>
                        {
                            Field = x => x.CreatedAt,
                            Direction = SortDirection.Descending
                        }
                    ],
                Cursor = cursor,
                PageSize = pageSize
            };

            return await repository.GetPagedAsync(specification);
        }

        public async Task<PostReaction> Create(AddPostReactionRequest request)
        {
            var model = mapper.Map<PostReaction>(request);
            return await repository.CreateAsync(model);
        }

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