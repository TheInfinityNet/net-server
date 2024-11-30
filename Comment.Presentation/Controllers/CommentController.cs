using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.Services.Comment.Application.DTOs.Requests;
using InfinityNetServer.Services.Comment.Application.DTOs.Responses;
using InfinityNetServer.Services.Comment.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Comment.Presentation.Controllers
{
    [ApiController]
    //[Route("comments")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly CommonProfileClient _commonProfileClient;

        public CommentController(ICommentService commentService, CommonProfileClient commonProfileClient)
        {
            _commentService = commentService;
            _commonProfileClient = commonProfileClient;
        }

        //[Authorize]
        [HttpGet("preview-comment/{postId}")]
        public async Task<IActionResult> GetTopCommentWithMostReplies(string postId)
        {
            var response = await _commentService.GetTopCommentWithMostRepliesAsync(postId);

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
        [HttpGet("{commentId}/replies")]
        public async Task<IActionResult> GetChildComments(Guid commentId)
        {
            if (commentId == Guid.Empty)
            {
                return BadRequest("Parent Comment ID is required.");
            }

            var childComments = await _commentService.GetChildCommentsAsync(commentId);

            var profilesTasks = childComments.Select(async comment =>
            {
                try
                {
                    var profileResponse = await _commonProfileClient.GetProfile(comment.ProfileId.ToString());

                    comment.Profile = new SimpleProfileResponse
                    {
                        Username = profileResponse.Name,
                        AvatarId = profileResponse.Avatar?.Id.ToString()
                    };
                }
                catch (BaseException ex)
                {
                    comment.Profile = null;
                }
                return comment;
            });

            await Task.WhenAll(profilesTasks);

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
        [HttpDelete("delete/{commentId}")]
        public async Task<IActionResult> DeleteComment(Guid commentId)
        {
            var response = await _commentService.DeleteCommentAsync(commentId);

            if (!response.IsDeleted)
                return NotFound(new { message = response.Message });

            return Ok(new { message = response.Message });
        }
        [HttpPut("update/{commentId}")]
        public async Task<IActionResult> UpdateComment(Guid commentId, [FromBody] UpdateCommentRequest request)
        {
            if (commentId == Guid.Empty)
            {
                return BadRequest("Invalid comment ID.");
            }

            var response = await _commentService.UpdateCommentAsync(commentId, request);

            if (!response.Success)
                return BadRequest(new { message = response.Message });

            return Ok(new { message = response.Message });
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
