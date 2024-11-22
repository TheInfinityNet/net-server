using AutoMapper;
using InfinityNetServer.Services.Reaction.Domain.Entities;
using InfinityNetServer.Services.Reaction.Domain.Repositories;
using InfinityNetServer.Services.Reaction.Infrastructure.Data;
using k8s.KubeConfigModels;
using Microsoft.EntityFrameworkCore;
using InfinityNetServer.Application.DTOs.Results;
using InfinityNetServer.Application.Post.Presentation.DTOs.Requests;
using InfinityNetServer.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Presentation.Services
{
    public class CommentReactionService(ICommentReactionRepository repository, ReactionDbContext context, IMapper mapper) : ICommentReactionService
    {
        private readonly ICommentReactionRepository _repository = repository;
        private readonly ReactionDbContext _context = context;

        private readonly IMapper _mapper = mapper;

        public async Task<CommentReaction> CreateAsync(AddCommentReactionRequest request)
        {
            var model = _mapper.Map<CommentReaction>(request);
            return await _repository.CreateAsync(model);
        }
        public async Task<List<CommandReacionGroupResult>> GetCommandReaction(string lstCommandId)
        {
            var lstId = lstCommandId.Split(',').Select(q => Guid.TryParse(q, out Guid postId) ? postId : Guid.Empty)
                .Where(q => q != Guid.Empty).ToList();
            if (lstId.Count == 0) return [];
            var request = await _context.CommentReactions
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