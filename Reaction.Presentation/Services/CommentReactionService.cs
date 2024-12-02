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
    public class CommentReactionService(
        ILogger<CommentReactionService> logger,
        ICommentReactionRepository repository, 
        ReactionDbContext context, IMapper mapper) : ICommentReactionService
    {

        public async Task<IList<(string commentId, IDictionary<ReactionType, int> countDetails)>> CountByCommentIdAsync(IList<string> commentIds)
        {
            var reactionCounts = await repository.CountByCommentIdAsync(commentIds.Select(Guid.Parse).ToList());
            return reactionCounts.Select(q => (q.commentId.ToString(), q.countDetails)).ToList();
        }

        public async Task<IList<CommentReaction>> GetAllByCommentIdsAndProfileIds(IList<(string commentId, string profileId)> commentIdsAndProfileIds)
            => await repository.GetAllByCommentIdsAndProfileIdsAsync(commentIdsAndProfileIds.Select(q => (Guid.Parse(q.commentId), Guid.Parse(q.profileId))).ToList());

        public async Task<CursorPagedResult<CommentReaction>> GetByCommentId
            (string commentId, string cursor, int pageSize, ReactionType type)
        {
            var specification = new SpecificationWithCursor<CommentReaction>
            {
                Criteria = reaction => reaction.CommentId == Guid.Parse(commentId) && reaction.Type.Equals(type) && !reaction.IsDeleted,

                OrderFields = [
                        new OrderField<CommentReaction>
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

        public async Task<CommentReaction> Create(AddCommentReactionRequest request)
        {
            var model = mapper.Map<CommentReaction>(request);
            return await repository.CreateAsync(model);
        }

        public async Task<List<CommandReacionGroupResult>> GetCommandReaction(string lstCommandId)
        {
            var lstId = lstCommandId.Split(',').Select(q => Guid.TryParse(q, out Guid postId) ? postId : Guid.Empty)
                .Where(q => q != Guid.Empty).ToList();
            if (lstId.Count == 0) return [];
            var request = await context.CommentReactions
                .Where(q => lstId.Any(p => p.Equals(q.CommentId)))
                .GroupBy(q => new { q.CommentId, q.Type })
                .Select(q => new CommandReacionGroupResult()
                {
                    Count = q.Count(),
                    CommentId = q.Key.CommentId,
                    Type = q.Key.Type,
                }).ToListAsync();
            return request;
        }
    }
}