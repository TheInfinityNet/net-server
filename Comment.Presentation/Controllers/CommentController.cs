using InfinityNetServer.Services.Comment.Application.Services;
using InfinityNetServer.Services.Comment.Application.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using InfinityNetServer.Services.Comment.Application.DTOs.Requests;

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

            if (count < 0)
            {
                return NotFound(new { Message = "No comments found for this post." });
            }

            var response = new CommentCountResponse(postId, count);
            return Ok(response);
        }

        [HttpPost("top-replied")]
        public async Task<IActionResult> GetTopCommentWithMostReplies([FromBody] Application.DTOs.Requests.TopRepliedCommentRequest request)
        {
            var response = await _commentService.GetTopCommentWithMostRepliesAsync(request.PostId);

            if (response == null)
                return NotFound(new { message = "No comments found for the given post ID." });

            return Ok(response);
        }

        [HttpPost("get-comments")]
        public async Task<IActionResult> GetCommentsForPost([FromBody] GetCommentsRequest request)
        {
            var response = await _commentService.GetCommentsForPostAsync(request);

            if (response.Comments == null || response.Comments.Count == 0)
                return NotFound(new { message = "No comments found for the given post ID." });

            return Ok(response);
        }
        //[HttpPost("get-child-comments")]
        //public async Task<ActionResult<GetChildCommentsResponse>> GetChildComments([FromBody] GetChildCommentsRequest request)
        //{
        //    if (request == null || request.Id == Guid.Empty)
        //    {
        //        return BadRequest("Invalid comment ID.");
        //    }

        //    var result = await _commentService.GetChildCommentsAsync(request.Id);

        //    if (result.ChildCommentIds.Count == 0)
        //    {
        //        return NotFound("No child comments found.");
        //    }

        //    return Ok(result);
        //}
        [HttpPost("add-comment")]
        public async Task<ActionResult<AddCommentResponse>> AddComment([FromBody] AddCommentRequest request)
        {
            if (request == null)
                return BadRequest("Invalid request data.");

            var response = await _commentService.AddCommentAsync(request);
            return Ok(response);
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteComment([FromBody] DeleteCommentRequest request)
        {
            var response = await _commentService.DeleteCommentAsync(request);

            if (!response.IsDeleted)
                return NotFound(new { message = response.Message });

            return Ok(new { message = response.Message });
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateComment([FromBody] UpdateCommentRequest request)
        {
            var response = await _commentService.UpdateCommentAsync(request);

            if (!response.Success)
                return BadRequest(new { message = response.Message });

            return Ok(new { message = response.Message });
        }
    }
}
