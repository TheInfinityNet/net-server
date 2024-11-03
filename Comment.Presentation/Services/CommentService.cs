using InfinityNetServer.Services.Comment.Application.Services;
using InfinityNetServer.Services.Comment.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Comment.Presentation.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<int> CountCommentsByPostIdAsync(Guid postId)
        {
            return await _commentRepository.CountByPostIdAsync(postId);
        }
    }
}
