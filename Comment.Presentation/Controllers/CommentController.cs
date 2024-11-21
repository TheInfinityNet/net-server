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
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost("comment-count")]
        public async Task<IActionResult> GetCommentCount([FromBody] GetPostIdRequest request)
        {
            try
            {
                var response = await _commentService.GetCommentCountAsync(request);
                return Ok(new { postId = response.PostId, commentCount = response.CommentCount });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }


        [HttpPost("preview-comment")]
        public async Task<IActionResult> GetTopCommentWithMostReplies([FromBody] GetPostIdRequest request)
        {
            var response = await _commentService.GetTopCommentWithMostRepliesAsync(request);

            if (response == null)
                return NotFound(new { message = "No comments found for the given post ID." });

            return Ok(response);
        }

        [HttpPost("comments-loading")]
        public async Task<IActionResult> GetCommentsForPost([FromBody] GetCommentsRequest request)
        {
            var response = await _commentService.GetCommentsForPostAsync(request);

            if (response.Comments == null || response.Comments.Count == 0)
                return NotFound(new { message = "No comments found for the given post ID." });

            return Ok(response);
        }
        [HttpPost("get-child-comments")]
        public async Task<IActionResult> GetChildComments([FromBody] GetChildCommentsRequest request)
        {
            if (request.ParentCommentId == Guid.Empty)
            {
                return BadRequest("Parent Comment ID is required.");
            }

            var childComments = await _commentService.GetChildCommentsAsync(request.ParentCommentId);

            if (childComments == null || !childComments.Any())
            {
                return NotFound("No child comments found.");
            }

            return Ok(childComments);
        }

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
