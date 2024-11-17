using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Comment.Domain.Repositories;
using InfinityNetServer.Services.Comment.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Comment.Infrastructure.Repositories
{
    public class CommentRepository(CommentDbContext context) : SqlRepository<Domain.Entities.Comment, Guid>(context), ICommentRepository
    {

        public async Task<IList<Domain.Entities.Comment>> GetAllMediaCommentAsync()
            => await context.Comments.Where(c => c.FileMetadataId != null).ToListAsync();
        public async Task<int> CountByPostIdAsync(Guid postId)
        {
            try
            {
                return await context.Comments.CountAsync(c => c.PostId == postId && !c.IsDeleted);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CountByPostIdAsync: {ex.Message}");
                throw;
            }
        }
        public async Task<Domain.Entities.Comment?> GetTopCommentWithMostRepliesAsync(Guid postId)
        {
            return await context.Comments
                .Where(c => c.PostId == postId && !c.IsDeleted && c.ParentId == null)
                .OrderByDescending(c => c.RepliesComments.Count)
                .FirstOrDefaultAsync();
        }
        public async Task<(List<Domain.Entities.Comment>, int)> GetCommentsByPostIdAsync(Guid postId, int pageSize, int pageNumber)
        {
            var query = context.Comments.Where(c => c.PostId == postId && !c.IsDeleted && c.ParentId == null);

            var totalCount = await query.CountAsync();

            var comments = await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((pageNumber - 1) * pageSize) 
                .Take(pageSize)                     
                .ToListAsync();

            return (comments, totalCount);
        }
        public async Task<List<Domain.Entities.Comment>> GetChildCommentsAsync(Guid parentCommentId)
        {
            return await context.Comments
                .Where(c => c.ParentId == parentCommentId && !c.IsDeleted)
                .ToListAsync();
        }
        public async Task<Domain.Entities.Comment> AddCommentAsync(Domain.Entities.Comment comment)
        {
            await context.Comments.AddAsync(comment);
            await context.SaveChangesAsync();
            return comment;
        }
        public async Task<bool> DeleteCommentAsync(Guid commentId, Guid deletedBy)
        {
            var comment = await context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment == null || comment.IsDeleted)
                return false;

            // Đánh dấu comment là đã xóa
            comment.IsDeleted = true;
            comment.DeletedBy = deletedBy;

            context.Comments.Update(comment);
            await context.SaveChangesAsync();

            return true;
        }
        public async Task<Domain.Entities.Comment?> GetByIdAsync(Guid commentId)
        {
            return await context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
        }

        public async Task<bool> UpdateCommentAsync(Guid commentId, string newContent)
        {
            var comment = await GetByIdAsync(commentId);

            if (comment == null || comment.IsDeleted)
                return false;

            // Cập nhật nội dung comment
            comment.Content.Text = newContent;

            context.Comments.Update(comment);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
