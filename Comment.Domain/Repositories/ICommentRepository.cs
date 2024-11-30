using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.Services.Comment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Comment.Domain.Repositories
{
    public interface ICommentRepository : ISqlRepository<Entities.Comment, Guid>
    {
        public Task<IList<Entities.Comment>> GetAllMediaCommentAsync();
        Task<int> CountCommentsByPostIdAsync(Guid postId);
        Task<Entities.Comment> GetTopCommentWithMostRepliesAsync(Guid postId);
        Task<(List<Entities.Comment> Comments, int TotalCount)> GetCommentsByPostIdAsync(Guid postId, int pageSize, int pageNumber);
        Task<Entities.Comment> AddCommentAsync(Entities.Comment comment);
        Task<bool> DeleteCommentAsync(Guid commentId);
        Task<bool> UpdateCommentAsync(Guid commentId, CommentContent newContent);
        Task<List<Entities.Comment>> GetChildCommentsAsync(Guid parentCommentId);
        Task<int> GetRepliesCommentAsync(Guid commentId);
        public Task<IList<Entities.Comment>> GetAllByPostIdAsync(Guid postId);
        Task<int> CountByPostIdAsync(Guid postId);
    }
}
