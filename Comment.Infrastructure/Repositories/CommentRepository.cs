using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Comment.Domain.Repositories;
using InfinityNetServer.Services.Comment.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Comment.Infrastructure.Repositories
{
    public class CommentRepository : SqlRepository<Domain.Entities.Comment, Guid>, ICommentRepository
    {

        private readonly CommentDbContext _context;

        public CommentRepository(CommentDbContext context) : base(context)
        {
            _context = context; // Khởi tạo _context để sử dụng sau này
        }

        // Phương thức đếm số lượng comment cho một post
        public async Task<int> CountByPostIdAsync(Guid postId)
        {
            return await _context.Comments.CountAsync(c => c.PostId == postId && !c.IsDeleted);
        }
    }
}
