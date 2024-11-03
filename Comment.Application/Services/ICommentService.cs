using InfinityNetServer.Services.Comment.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Comment.Application.Services
{
    public interface ICommentService
    {
        Task<int> CountCommentsByPostIdAsync(Guid postId);
    }
}
