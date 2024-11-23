using AutoMapper;
using InfinityNetServer.Services.Reaction.Domain.Entities;
using InfinityNetServer.Services.Reaction.Domain.Repositories;
using InfinityNetServer.Services.Reaction.Infrastructure.Data;
using k8s.KubeConfigModels;
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
    public class CommentReactionService(ICommentReactionRepository repository, ReactionDbContext context, IMapper mapper) : ICommentReactionService
    {

        public async Task<int> CountByCommentIdAndType(string commentId, ReactionType type)
            => await repository.CountByCommentIdAndType(Guid.Parse(commentId), type);

        public async Task<CommentReaction> CreateAsync(AddCommentReactionRequest request)
        {
            var model = mapper.Map<CommentReaction>(request);
            return await repository.CreateAsync(model);
        }

        public async Task<CommentReaction> GetByCommentIdAndProfileId(string commentId, string profileId)
            => await repository.GetByCommentIdAndProfileId(Guid.Parse(commentId), Guid.Parse(profileId));

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