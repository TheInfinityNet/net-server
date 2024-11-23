using AutoMapper;
using InfinityNetServer.Services.Reaction.Domain.Entities;
using InfinityNetServer.Services.Reaction.Domain.Repositories;
using InfinityNetServer.Services.Reaction.Infrastructure.Data;
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
    public class PostReactionService(IPostReactionRepository repository, IMapper mapper, ReactionDbContext context) : IPostReactionService
    {
        private readonly IPostReactionRepository _repository = repository;
        private readonly ReactionDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        public async Task<PostReaction> CreatePostReaction(AddPostReactionRequest request)
        {
            var model = _mapper.Map<PostReaction>(request);
            return await _repository.CreateAsync(model);
        }
        public async Task<List<PostReactionGroupResult>> GetPostReactions(string lstPostId)
        {
            var lstId = lstPostId.Split(',').Select(q => Guid.TryParse(q, out Guid postId) ? postId : Guid.Empty)
                .Where(q => q != Guid.Empty).ToList();
            if (lstId.Count == 0) return [];
            var request = await _context.PostReactions
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