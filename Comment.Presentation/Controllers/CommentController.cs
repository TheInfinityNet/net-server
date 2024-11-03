using InfinityNetServer.Services.Comment.Application.Services;
using InfinityNetServer.Services.Comment.Application.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Comment.Presentation.Controllers
{
    [ApiController]
    [Route("api/comments")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("count/{postId}")]
        public async Task<ActionResult<CommentCountResponse>> GetCommentCountByPostId(Guid postId)
        {
            var count = await _commentService.CountCommentsByPostIdAsync(postId);

            if (count < 0) // Kiểm tra nếu không có bình luận
            {
                return NotFound(new { Message = "No comments found for this post." });
            }

            var response = new CommentCountResponse(postId, count);
            return Ok(response);
        }
    }
}
